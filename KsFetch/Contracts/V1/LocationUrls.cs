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
    public sealed class LocationUrls
    {

        [DataMember(Name = "web", IsRequired = true)]
        public LocationUrlWeb Web { get; set; }

        [DataMember(Name = "api", IsRequired = true)]
        public LocationUrlApi Api { get; set; }

    }

}
