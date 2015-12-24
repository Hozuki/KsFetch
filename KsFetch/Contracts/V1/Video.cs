using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KsFetch.Contracts.V1
{

    [DataContract()]
    public sealed class Video
    {

        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        [DataMember(Name = "high", IsRequired = true)]
        public string High { get; set; }

        [DataMember(Name = "base", IsRequired = true)]
        public string Base { get; set; }

        [DataMember(Name = "webm", IsRequired = true)]
        public string WebM { get; set; }

        [DataMember(Name = "width", IsRequired = true)]
        public int Width { get; set; }

        [DataMember(Name = "height", IsRequired = true)]
        public int Height { get; set; }

        [DataMember(Name = "frame", IsRequired = true)]
        public string Frame { get; set; }

    }

}
