using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraceabilityWebApi.Models;

namespace TraceabilityWebApi.Controllers
{
    [Route("api/Waga")]
    public class WagaController : ApiController
    {
        // Methods
        [HttpGet]
        public string GetWage([FromBody] Waga wage)
        {
            WagaCommunication.Initialize("COM9", 4800);
            wage.waga = WagaCommunication.Read();
            WagaCommunication.Close();
            return wage.waga;
        }

        [HttpGet]
        public IEnumerable<Waga> GetWages()
        {
            List<Waga> list = new List<Waga>();
            WagaCommunication.Initialize("COM9", 4800);
            Waga item = new Waga
            {
                waga = WagaCommunication.Read()
            };
            list.Add(item);
            WagaCommunication.Close();
            return list;
        }


    }
}
