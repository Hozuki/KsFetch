using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using KsFetch.Contracts.V1;
using System.Runtime.Serialization;
using HtmlAgilityPack;

namespace KsFetch
{

    public sealed class KsFetchWebClient
    {

        public KsFetchWebClient()
        {
        }

        public void Login(string email, string password)
        {
            var loginObject = new LogInInfo(email, password);
            var loginSerializer = new DataContractJsonSerializer(typeof(LogInInfo));
            var dataString = loginSerializer.WriteObject(loginObject);
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            Action<Stream> postLoginData = (s) =>
            {
                s.Write(dataBytes, 0, dataBytes.Length);
                s.Flush();
            };
            var request = CreatePostRequest(API_BASE + "/xauth/access_token", postLoginData, dataBytes.Length, new Dictionary<string, string>
            {
                ["client_id"] = "2II5GGBZLOOZAA5XBU1U0Y44BU57Q58L8KOGM7H0E0YFHP3KTG"
            });
            var responseText = GetResponseText(request);
            EnsureToken(responseText);
            var tokenSerializer = Serializers.GetSerializer(typeof(TokenResponse));
            var responseObject = tokenSerializer.ReadClass<TokenResponse>(responseText);
            _accessToken = responseObject.AccessToken;
        }

        public bool IsLoggedIn
        {
            get
            {
                return _accessToken != null;
            }
        }

        public string AccessToken => _accessToken;

        public Project GetProject(string projectAlias)
        {
            return GetProjectByAliasOrID(projectAlias);
        }

        public Project GetProject(long projectID)
        {
            return GetProjectByAliasOrID(projectID.ToString());
        }

        public SearchResponse SearchInCategory(CategoryKind categoryKind, SortMethod sortMethod, uint page)
        {
            // https://www.kickstarter.com/discover/advanced?category_id=3&sort=popularity&page=2 (comics)
            // https://www.kickstarter.com/discover/categories/theater?ref=category_modal&sort=popularity (theater)
            // https://www.kickstarter.com/discover/advanced?sort=popularity (everything)
            // https://www.kickstarter.com/discover/advanced?ref=category_modal&sort=popularity&starred=1 (starred)
            // https://www.kickstarter.com/discover/recommended?ref=category_modal&sort=popularity (recommended)

            string baseUrl = null;
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            switch (categoryKind)
            {
                case CategoryKind.Recommended:
                    baseUrl = CombineUrl(SEARCH_BASE, "discover/recommended");
                    throw new NotImplementedException();
                case CategoryKind.Starred:
                    baseUrl = CombineUrl(SEARCH_BASE, "discover/advanced");
                    queryParams["starred"] = "1";
                    throw new NotImplementedException();
                case CategoryKind.Everything:
                    baseUrl = CombineUrl(SEARCH_BASE, "discover/advanced");
                    break;
                default:
                    baseUrl = CombineUrl(SEARCH_BASE, "discover/categories", AssociatedTextAttribute.ExtractFromEnum(categoryKind));
                    //baseUrl = CombineUrl(SEARCH_BASE, "discover/advanced");
                    //queryParams["category_id"] = ((int)categoryKind).ToString();
                    break;
            }
            queryParams["sort"] = AssociatedTextAttribute.ExtractFromEnum(sortMethod);
            queryParams["page"] = page.ToString();
            var request = CreateGetRequest(baseUrl, queryParams);
            var responseText = GetResponseText(request);
            EnsureToken(responseText);
            var responseSerializer = Serializers.GetSerializer(typeof(SearchResponse));
            return responseSerializer.ReadClass<SearchResponse>(responseText);
        }

        public SearchResponse SearchInSubCategory(SubCategoryKind subCategoryKind, SortMethod sortMethod, uint page)
        {
            // https://www.kickstarter.com/discover/categories/comics/anthologies?sort=popularity (comics-anthologies)
            throw new NotImplementedException();
        }

        private Project GetProjectByAliasOrID(string key)
        {
            var baseUrl = CombineUrl(API_BASE, "v1/projects", key);
            var request = CreateGetRequest(baseUrl);
            var responseText = GetResponseText(request);
            EnsureToken(responseText);
            var responseSerializer = Serializers.GetSerializer(typeof(Project));
            return responseSerializer.ReadClass<Project>(responseText);
        }

        private HttpWebRequest CreateBasicRequest(string baseUrl, Dictionary<string, string> queryParameters)
        {
            if (IsLoggedIn)
            {
                if (queryParameters == null)
                {
                    queryParameters = new Dictionary<string, string>();
                }
                queryParameters["oauth_token"] = _accessToken;
            }
            var realUrl = baseUrl;
            if (queryParameters != null && queryParameters.Count > 0)
            {
                realUrl += "?";
                foreach (var param in queryParameters)
                {
                    realUrl += $"{param.Key}={param.Value}&";
                }
                realUrl = realUrl.Substring(0, realUrl.Length - 1);
            }
            var request = WebRequest.CreateHttp(realUrl);
            request.Accept = "application/json; charset=utf-8";
            request.ContentType = "application/json";
            request.UserAgent = "KsFetch";
            return request;
        }

        private HttpWebRequest CreateGetRequest(string baseUrl, Dictionary<string, string> queryParameters = null)
        {
            var request = CreateBasicRequest(baseUrl, queryParameters);
            request.Method = "GET";
            return request;
        }

        private HttpWebRequest CreatePostRequest(string baseUrl, Action<Stream> writing, long contentLength, Dictionary<string, string> queryParameters = null)
        {
            var request = CreateBasicRequest(baseUrl, queryParameters);
            request.Method = "POST";
            request.ContentLength = contentLength;
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    if (writing != null)
                    {
                        writing(stream);
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.TrustFailure)
            {
                // If there is a proxy (e.g. Fiddler) between local and remote endpoints, it may trigger a TrustFailure due to
                // illegal SSL/TLS certificate. In this case, ignore the exception and keep writing nothing.
            }
            return request;
        }

        private Stream GetProperStream(Stream originalStream, string contentEncoding)
        {
            if (contentEncoding == null || contentEncoding.Length <= 0)
            {
                return originalStream;
            }
            var stream = originalStream;
            switch (contentEncoding)
            {
                case "gzip":
                    stream = new GZipStream(stream, CompressionMode.Decompress, false);
                    break;
                case "deflate":
                    stream = new DeflateStream(stream, CompressionMode.Decompress, false);
                    break;
                default:
                    break;
            }
            return stream;
        }

        private string GetResponseText(HttpWebRequest request, Encoding encoding)
        {
            var r = string.Empty;
            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
            {
                response = ex.Response as HttpWebResponse;
            }
            using (var stream = GetProperStream(response.GetResponseStream(), response.Headers["Content-Type"]))
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    r = reader.ReadToEnd();
                }
            }
            if (response != null)
            {
                response.Dispose();
            }
            return r;
        }

        private string GetResponseText(HttpWebRequest request)
        {
            return GetResponseText(request, Encoding.UTF8);
        }

        private static string CombineUrl(string baseUrl, params string[] parts)
        {
            var url = baseUrl.TrimEnd('/');
            for (var i = 0; i < parts.Length; ++i)
            {
                url += "/" + parts[i].Trim('/');
            }
            return url;
        }

        private static void EnsureToken(string jsonText)
        {
            var tokenSerializer = Serializers.GetSerializer(typeof(TokenResponse));
            TokenResponse responseObject = null;
            try
            {
                responseObject = tokenSerializer.ReadClass<TokenResponse>(jsonText);
            }
            catch (SerializationException ex)
            {
                var errorMessage = ex.Message + Environment.NewLine;
                var document = new HtmlDocument();
                document.LoadHtml(jsonText);
                if (document.ParseErrors.Count() <= 0)
                {
                    errorMessage += document.DocumentNode.ChildNodes["html"].ChildNodes["body"].Element("div").InnerText.Trim();
                    throw new KickstarterApiException(500, errorMessage, string.Empty);
                }
                else
                {
                    throw ex;
                }
            }
            if (responseObject == null)
            {
                throw new KickstarterApiException(0, "Deserialization failed.", jsonText);
            }
            else if (responseObject.ErrorMessages != null)
            {
                throw new KickstarterApiException(responseObject.HttpCode, responseObject.ErrorMessages.Join(Environment.NewLine), responseObject.Disclaimer);
            }
        }

        private static readonly string API_BASE = "https://api.kickstarter.com";
        private static readonly string SEARCH_BASE = "https://www.kickstarter.com";

        private CookieContainer _cookies = new CookieContainer();
        private string _accessToken = null;

    }

}
