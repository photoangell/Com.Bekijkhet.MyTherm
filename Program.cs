using System;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                Pi.Init<BootstrapWiringPi>();
                var pin = (GpioPin)Pi.Gpio[BcmPin.Gpio04];
                var dht = new DHT11(pin);
                while (true) {
                    try {
                        var d = dht.ReadDataWithRetries();
                        Console.WriteLine(DateTime.Now);
                        Console.WriteLine(" temp: " + d.TempCelsius);
                        Console.WriteLine(" hum: " + d.Humidity);
                        Console.WriteLine(" heat index: " + d.HeatIndex);
                    } catch (DHTException e) {
                        Console.Error.WriteLine(e.Message + " - " + e.StackTrace);
                    } 
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message + " - " + e.StackTrace);
            }
        }
    }
}