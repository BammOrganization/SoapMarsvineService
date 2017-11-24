using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MarsvinServiceSOAP
{
    [DataContract]
    public class Measurement
    {
        [DataMember]
        public int Id;

        [DataMember]
        public DateTime Time;

        [DataMember]
        public int dB;

        [DataMember]
        public string ImageLink;
    }
}