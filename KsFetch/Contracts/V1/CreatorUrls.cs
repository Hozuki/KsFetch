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
    public sealed class CreatorUrls
    {

        [DataMember(Name = "web", IsRequired = true)]
        public CreatorUrlWeb Web { get; set; }

        [DataMember(Name = "api", IsRequired = true)]
        public CreatorUrlApi Api { get; set; }

    }

}
