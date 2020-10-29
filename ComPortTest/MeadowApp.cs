using System;
using System.Linq;
using System.Text;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace ComPortTest
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        ISerialMessagePort port;
        CharacterDisplay display;
        II2cBus bus;
        

        public MeadowApp()
        {
            Initialize();
            onboardLed.SetColor(Color.Green);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            bus = Device.CreateI2cBus();

            display = new CharacterDisplay(bus, 39, 2, 16);

            port = Device.CreateSerialMessagePort(Device.SerialPortNames.Com4, suffixDelimiter: new byte[] { 10 }, preserveDelimiter: true, 1000000, 8, Parity.None, StopBits.One);
            port.Open();
            port.MessageReceived += Port_MessageReceived;
        }

        private void Port_MessageReceived(object sender, SerialMessageData e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);
            foreach (byte b in msg) Console.WriteLine(b);
            Console.WriteLine(msg);
            display.ClearLines();
            display.Write(msg);
            onboardLed.SetColor(Color.Red);             //So i knew it was receiving data before I connected display
            Thread.Sleep(500);
            onboardLed.SetColor(Color.Blue);
            port.Write(Encoding.UTF8.GetBytes($"Received and acknowledging! Msg: {msg}"));  //There is small issue that messages contain LF at the end and it's sent back too
                                                                                            //but nah, in target use it won't matter

        }
    }
}
