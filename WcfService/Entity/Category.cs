using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService.Entity
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public int IconId { get; set; }

        [DataMember]
        public long UserId { get; set; }
    }
}