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
    public sealed class ProfileFeatureImageAttributesImageUrls
    {

        [DataMember(Name = "default", IsRequired = true)]
        public string Default { get; set; }

        [DataMember(Name = "baseball_card", IsRequired = true)]
        public string BaseballCard { get; set; }

    }

}
