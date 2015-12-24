using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch
{

    [DataContract()]
    class LogInInfo
    {

        public LogInInfo(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [DataMember(Name = "email", IsRequired = true)]
        public string Email { get; set; }

        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }

    }

}
