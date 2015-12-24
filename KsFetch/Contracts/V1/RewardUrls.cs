using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class RewardUrls
    {

        [DataMember(Name = "api", IsRequired = true)]
        public RewardUrlApi Api { get; set; }

    }

}
