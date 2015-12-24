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
    public sealed class SearchResponse
    {

        [DataMember(Name = "projects", IsRequired = true)]
        public ProjectAbstract[] Projects { get; set; }

        [DataMember(Name = "total_hits", IsRequired = true)]
        public long TotalHits { get; set; }

        [DataMember(Name = "seed", IsRequired = false)]
        public string Seed { get; set; }

        [DataMember(Name = "search_url", IsRequired = true)]
        public string SearchUrl { get; set; }

        [DataMember(Name = "has_more", IsRequired = true)]
        public bool HasMore { get; set; }

    }

}
