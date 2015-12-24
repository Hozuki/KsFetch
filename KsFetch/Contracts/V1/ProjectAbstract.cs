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
    public class ProjectAbstract
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "photo", IsRequired = true)]
        public Photo Photo { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "blurb", IsRequired = true)]
        public string Blurb { get; set; }

        [DataMember(Name = "goal", IsRequired = true)]
        public double Goal { get; set; }

        [DataMember(Name = "pledged", IsRequired = true)]
        public double Pledged { get; set; }

        [DataMember(Name = "state", IsRequired = true)]
        public string State { get; set; }

        [DataMember(Name = "slug", IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "disable_communication", IsRequired = true)]
        public bool DisableCommunication { get; set; }

        [DataMember(Name = "country", IsRequired = true)]
        public string Country { get; set; }

        [DataMember(Name = "currency", IsRequired = true)]
        public string CurrencyCode { get; set; }

        [DataMember(Name = "currency_symbol", IsRequired = true)]
        public string CurrencySymbol { get; set; }

        [DataMember(Name = "currency_trailing_code", IsRequired = true)]
        public bool CurrencyTrailingCode { get; set; }

        [DataMember(Name = "deadline", IsRequired = true)]
        public long DeadlineTimestamp { get; set; }

        [DataMember(Name = "state_changed_at", IsRequired = true)]
        public long StateChangedAtTimestamp { get; set; }

        [DataMember(Name = "created_at", IsRequired = true)]
        public long CreatedAtTimestamp { get; set; }

        [DataMember(Name = "launched_at", IsRequired = true)]
        public long LaunchedAtTimestamp { get; set; }

        [DataMember(Name = "backers_count", IsRequired = true)]
        public uint BackersCount { get; set; }

        [DataMember(Name = "static_usd_rate", IsRequired = true)]
        public string StaticUsdRate { get; set; }

        [DataMember(Name = "usd_pledged", IsRequired = true)]
        public string UsdPledged { get; set; }

        [DataMember(Name = "creator", IsRequired = true)]
        public Creator Creator { get; set; }

        [DataMember(Name = "location", IsRequired = false)]
        public Location Location { get; set; }

        [DataMember(Name = "category", IsRequired = true)]
        public Category Category { get; set; }

        [DataMember(Name = "profile", IsRequired = true)]
        public Profile Profile { get; set; }

        [DataMember(Name = "spotlight", IsRequired = true)]
        public bool Spotlight { get; set; }

        [DataMember(Name = "urls", IsRequired = true)]
        public ProjectUrls Urls { get; set; }

    }

}
