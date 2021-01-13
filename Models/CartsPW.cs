using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class CartsPW
    {
        public string Nr_wozka { get; set; }
        public string Nm { get; set; }
        public string Material { get; set; }
        public string Typ_cewki { get; set; }
        public string Kolor_cewki { get; set; }
        public string Nazwa_maszyny { get; set; }
        public string Nazwa_maszyny2 { get; set; }

        public string TS_KOM1 { get; set; }
        public string TS_PW1 { get; set; }
        public string TS_KOM2 { get; set; }
        public string TS_PW2 { get; set; }
        public string PartiaString { get; set; }
    }
}