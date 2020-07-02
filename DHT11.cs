using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    public sealed class DHT11 : DHT
    {
        public DHT11(GpioPin dataPin) : base(dataPin) { }
    
        public override DHTData ReadData()
        {
            float t = 0;
            float h = 0;

            Read();
            t = _data[2];
            h = _data[0];

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