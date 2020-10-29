using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComPortTestPC
{
    class Program
    {
        static SerialPort port;
        static void Main(string[] args)
        {
            port = new SerialPort("COM4", 1000000, Parity.None, 8, StopBits.One);
            port.Open();
            while (true)
            {
                string message = Console.ReadLine();
                port.WriteLine(message);
                port.DataReceived += Port_DataReceived;
            }
        }

        private static void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine(port.ReadLine());
        }
    }
}
