using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Entity
{
    public class CIcon
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public byte[] Binary { get; set; }
    }
}