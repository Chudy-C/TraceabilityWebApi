using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Communications
{
    public static class ZebraCommunication
    {

        private static SerialPort serialConnection;

        // Methods
        public static void Close()
        {
            serialConnection.Close();
        }

        public static void Initialize(string port, int baud)
        {
            serialConnection = new SerialPort(port, baud, Parity.None, 8, StopBits.One);
            serialConnection.Open();
            byte[] data = new byte[] { 0x53, 0x49, 0x0D, 0x0A };
            Write(data);
        }

        public static string Read() =>
            serialConnection.ReadLine();

        public static void Write(byte[] data)
        {
            serialConnection.Write(data, 0, 4);
        }

    }
}