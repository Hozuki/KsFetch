using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class Project : ProjectAbstract
    {

        [DataMember(Name = "is_starred", IsRequired = true)]
        public bool IsStarred { get; set; }

        [DataMember(Name = "is_backing", IsRequired = true)]
        public bool IsBacking { get; set; }

        [DataMember(Name = "video", IsRequired = false)]
        public Video Video { get; set; }

        [DataMember(Name = "updated_at", IsRequired = false)]
        public long UpdatedAtTimestamp { get; set; }

        [DataMember(Name = "comments_count", IsRequired = false)]
        public uint CommentsCount { get; set; }

        [DataMember(Name = "updates_count", IsRequired = false)]
        public uint UpdatesCount { get; set; }

        [DataMember(Name = "rewards", IsRequired = false)]
        public Reward[] Rewards { get; set; }

    }

}
