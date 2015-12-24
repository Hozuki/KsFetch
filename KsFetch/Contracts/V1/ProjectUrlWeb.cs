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
    public sealed class ProjectUrlWeb
    {

        [DataMember(Name = "project", IsRequired = true)]
        public string Project { get; set; }

        [DataMember(Name = "rewards", IsRequired = true)]
        public string Rewards { get; set; }

    }

}
