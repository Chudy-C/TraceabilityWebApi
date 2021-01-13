using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class Response
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public int ValInt { get; set; }
        public string ValString { get; set; }

    }
}