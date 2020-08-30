using System;
using System.Collections.Generic;
using System.Text;

namespace CovidStatus.Shared.Entities
{
    public class County
    {
        public byte CountyID { get; set; }
        public string CountyName { get; set; }
        public int Population { get; set; }
    }
}
