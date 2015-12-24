using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class Reward
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "minimum", IsRequired = true)]
        public double Minimum { get; set; }

        [DataMember(Name = "reward", IsRequired = true)]
        public string RewardContent { get; set; }

        [DataMember(Name = "description", IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "shipping_enabled", IsRequired = false)]
        public bool ShippingAvailable { get; set; }

        [DataMember(Name = "estimated_delivery_on", IsRequired = false)]
        public long EstimatedDeliveryOnTimestamp { get; set; }

        [DataMember(Name = "project_id", IsRequired = false)]
        public long ProjectID { get; set; }

        [DataMember(Name = "backers_count", IsRequired = false)]
        public uint BackersCount { get; set; }

        [DataMember(Name = "updated_at", IsRequired = false)]
        public long UpdatedAtTimestamp { get; set; }

        [DataMember(Name = "urls", IsRequired = false)]
        public RewardUrls Urls { get; set; }

    }

}
