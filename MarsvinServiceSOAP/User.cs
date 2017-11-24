using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MarsvinServiceSOAP
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Mail;

        [DataMember]
        public int PhoneNo;
    }
}