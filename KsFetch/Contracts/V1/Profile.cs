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
    public sealed class Profile
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "project_id", IsRequired = true)]
        public ulong ProjectID { get; set; }

        [DataMember(Name = "state", IsRequired = true)]
        public string State { get; set; }

        [DataMember(Name = "state_changed_at", IsRequired = true)]
        public long StateChangedAtTimestamp { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "blurb", IsRequired = true)]
        public string Blurb { get; set; }

        [DataMember(Name = "background_color", IsRequired = true)]
        public string BackgroundColor { get; set; }

        [DataMember(Name = "text_color", IsRequired = true)]
        public string TextColor { get; set; }

        [DataMember(Name = "link_background_color", IsRequired = true)]
        public string LinkBackgroundColor { get; set; }

        [DataMember(Name = "link_text_color", IsRequired = true)]
        public string LinkTextColor { get; set; }

        [DataMember(Name = "link_text", IsRequired = true)]
        public string LinkText { get; set; }

        [DataMember(Name = "link_url", IsRequired = true)]
        public string LinkUrl { get; set; }

        [DataMember(Name = "show_feature_image", IsRequired = true)]
        public bool ShowFeatureImage { get; set; }

        [DataMember(Name = "background_image_opacity", IsRequired = true)]
        public double BackgroundImageOpacity { get; set; }

        [DataMember(Name = "should_show_feature_image", IsRequired = true)]
        public bool ShouldShowFeatureImage { get; set; }

        [DataMember(Name = "feature_image_attributes", IsRequired = true)]
        public ProfileFeatureImageAttributes FeatureImageAttributes { get; set; }

    }
}
