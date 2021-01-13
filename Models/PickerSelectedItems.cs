using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class PickerSelectedItems
    {
        public string PIdMachinePz { get; set; }
        public string PNm { get; set; }
        public string PMaterial { get; set; }
        public string PCoilType { get; set; }
        public string PCoilColor { get; set; }
    }
}