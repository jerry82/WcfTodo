using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService.Entity
{
    [DataContract]
    public class LoginResult
    {
        [DataMember]
        public string ResultString { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public string Username { get; set; }
    }
}