using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class RewardUrlApi
    {

        [DataMember(Name = "reward", IsRequired = true)]
        public string Reward { get; set; }

    }

}
