using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch
{

    public sealed class KickstarterApiException : Exception
    {

        public KickstarterApiException(int httpCode, string errorMessage, string disclaimer)
            : base(errorMessage)
        {
            HttpCode = httpCode;
            Disclaimer = disclaimer;
        }

        public string Disclaimer { get; }

        public int HttpCode { get; }

    }

}
