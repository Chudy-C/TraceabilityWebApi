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
        private static SerialPort serialConnection;

        [HttpGet]
        public IEnumerable<Waga> ReadWage()
        {
            serialConnection = new SerialPort("COM10", 115200, Parity.None, 8, StopBits.One);

            List<Waga> wagaList = new List<Waga>();
            Waga waga = new Waga();

            serialConnection.Open();
            serialConnection.Write(new byte[] { 0x53, 0x49, 0x0D, 0x0A }, 0, 4);


            waga.waga = serialConnection.ReadLine();

            wagaList.Add(waga);

            return wagaList;
        }

    }
}
