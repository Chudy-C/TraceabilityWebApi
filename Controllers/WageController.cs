using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraceabilityWebApi.Models;
using System.Net.Mail;

namespace TraceabilityWebApi.Controllers
{
   
    public class WageController : ApiController
    {

        [HttpGet]
        public string GetWage([FromBody] Waga wage)
        {
            WagaCommunication.Initialize("COM11", 4800);
            wage.waga = WagaCommunication.Read();
            WagaCommunication.Close();
            return wage.waga;
        }

        [HttpGet]
        public IEnumerable<Waga> GetWages()
        {
            List<Waga> list = new List<Waga>();
            try
            {
                WagaCommunication.Initialize("COM11", 4800);
                Waga item = new Waga
                {
                    waga = WagaCommunication.Read()
                };
                list.Add(item);
                WagaCommunication.Close();
                
            }
            catch (Exception ex)
            {
                WagaCommunication.Close();
            }
            return list;
        }

    }
}
