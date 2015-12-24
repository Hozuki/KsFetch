using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class TokenResponse
    {

        [DataMember(Name = "error_messages", IsRequired = false)]
        public string[] ErrorMessages { get; set; }

        [DataMember(Name = "http_code", IsRequired = false)]
        public int HttpCode { get; set; }

        [DataMember(Name = "ksr_code", IsRequired = false)]
        public string KsrCode { get; set; }

        [DataMember(Name = "disclaimer", IsRequired = false)]
        public string Disclaimer { get; set; }

        [DataMember(Name = "access_token", IsRequired = false)]
        public string AccessToken { get; set; }

    }

}
