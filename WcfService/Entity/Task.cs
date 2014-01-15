using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService.Entity
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public bool Completed { get; set; }

        [DataMember]
        public long CatId { get; set; }
    }
}