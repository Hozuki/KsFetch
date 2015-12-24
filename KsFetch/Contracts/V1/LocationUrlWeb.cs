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
    public sealed class LocationUrlWeb
    {

        [DataMember(Name = "discover", IsRequired = true)]
        public string Discover { get; set; }

        [DataMember(Name = "location", IsRequired = true)]
        public string Location { get; set; }

    }

}
