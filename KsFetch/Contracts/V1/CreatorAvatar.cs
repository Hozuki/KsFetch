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
    public sealed class CreatorAvatar
    {

        [DataMember(Name = "thumb", IsRequired = true)]
        public string Thumb { get; set; }

        [DataMember(Name = "small", IsRequired = true)]
        public string Small { get; set; }

        [DataMember(Name = "medium", IsRequired = true)]
        public string Medium { get; set; }

    }

}
