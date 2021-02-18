using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraceabilityWebApi.Communications;
using TraceabilityWebApi.Models;

namespace TraceabilityWebApi.Controllers
{
    public class ScaleController : ApiController
    {
        [HttpGet]
        public IEnumerable<Waga> GetWages()
        {
            List<Waga> wageViewModelList = new List<Waga>();

            SerialCommunication.Initialize();

            wageViewModelList.Add(new Waga()
            {
                waga = SerialCommunication.Read()
            });

            SerialCommunication.Close();
            return wageViewModelList;
        }

        [HttpGet]
        public string GetWage([FromBody] Waga wage)
        {
            SerialCommunication.Initialize();
            wage.waga = SerialCommunication.Read();
            SerialCommunication.Close();
            return wage.waga;
        }
    }
}
