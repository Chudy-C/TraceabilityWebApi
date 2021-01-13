using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class StorageCarts
    {
        public string Nr_wozka { get; set; }
        public string CoilType { get; set; }
        public string CoilColor { get; set; }
        public string TS_MAG { get; set; }
        public string TS_MAG_PZ { get; set; }
        public int Id_machine_PW { get; set; }
        public string TS_MAG_PW { get; set; }
        public string TS_OUT { get; set; }

    }
}