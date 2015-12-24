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
    public sealed class Photo
    {

        [DataMember(Name = "key", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "full", IsRequired = true)]
        public string Full { get; set; }

        [DataMember(Name = "ed", IsRequired = true)]
        public string Ed { get; set; }

        [DataMember(Name = "med", IsRequired = true)]
        public string Medium { get; set; }

        [DataMember(Name = "little", IsRequired = true)]
        public string Little { get; set; }

        [DataMember(Name = "small", IsRequired = true)]
        public string Small { get; set; }

        [DataMember(Name = "thumb", IsRequired = true)]
        public string Thumb { get; set; }

        [DataMember(Name = "1024x768", IsRequired = true)]
        public string W1024H768 { get; set; }

        [DataMember(Name = "1536x1152", IsRequired = true)]
        public string W1536H1152 { get; set; }

    }

}
