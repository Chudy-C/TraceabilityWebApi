using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public static class WagaCommunication
    {
        private static SerialPort serialConnection;
        
        public static void Initialize(string port)
        {
            serialConnection = new SerialPort(port, 115200, Parity.None , 8 , StopBits.One);
        }

        public static void Write(string data)
        {
            serialConnection.Write(data);
        }
    }
}