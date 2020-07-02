using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    public class DHT22 : DHT
    {
        public DHT22(GpioPin dataPin) : base(dataPin) { }
    
        public override DHTData ReadData()
        {
            float t = 0;
            float h = 0;

            Read();
            t = _data[2] & 0x7F;
            t *= 256;
            t += _data[3];
            t /= 10;
            if ((_data[2] & 0x80) != 0)
            {
                t *= -1;
            }
            h = _data[0];
            h *= 256;
            h += _data[1];
            h /= 10;

            return new DHTData()
            {
                TempCelsius = t,
                TempFahrenheit = TemperatureUtils.ConvertCtoF(t),
                Humidity = h,
                HeatIndex = TemperatureUtils.ComputeHeatIndexFromCelsius(t, h)
            };
        }
    }
}