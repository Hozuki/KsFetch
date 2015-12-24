﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class ProfileFeatureImageAttributes
    {

        [DataMember(Name = "image_urls", IsRequired = true)]
        public ProfileFeatureImageAttributesImageUrls ImageUrls { get; set; }

    }

}
