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
    public sealed class Creator
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "slug", IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "avatar", IsRequired = true)]
        public CreatorAvatar Avatar { get; set; }

        [DataMember(Name = "urls", IsRequired = true)]
        public CreatorUrls Urls { get; set; }

    }

}
