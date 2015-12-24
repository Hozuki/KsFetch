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
    public sealed class Location
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "slug", IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "short_name", IsRequired = true)]
        public string ShortName { get; set; }

        [DataMember(Name = "displayable_name", IsRequired = true)]
        public string DisplayableName { get; set; }

        [DataMember(Name = "country", IsRequired = true)]
        public string Country { get; set; }

        [DataMember(Name = "state", IsRequired = true)]
        public string State { get; set; }

        [DataMember(Name = "type", IsRequired = true)]
        public string Type { get; set; }

        [DataMember(Name = "is_root", IsRequired = true)]
        public bool IsRoot { get; set; }

        [DataMember(Name = "urls", IsRequired = true)]
        public LocationUrls Urls { get; set; }

    }

}
