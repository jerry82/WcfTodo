using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService.Entity
{
    [DataContract]
    public class ServiceDataFault
    {
        [DataMember]
        public string Issue { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
}