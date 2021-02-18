using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Communications
{
    public class SerialCommunication
    {
        private static SerialPort serialConnection;

        public static void Initialize()
        {
            serialConnection = new SerialPort("COM11", 9600 ,Parity.None, 8, StopBits.One);
            serialConnection.Open();
            SerialCommunication.Write(new byte[4] { 0x53, 0x49, 0x0D, 0x0A});
        }

        public static void Write(byte[] data)
        {
            serialConnection.Write(data,0,4);
        }

        public static string Read()
        {
            return serialConnection.ReadLine();
        }

        public static void Close()
        {
            serialConnection.Close();
        }
    }
}