using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService.Entity
{
    [DataContract]
    public class CIcon
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string ImageUri { get; set; }
        //public byte[] Binary { get; set; }
    }
}