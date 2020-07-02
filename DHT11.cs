using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    public sealed class DHT11 : DHT
    {
        public DHT11(GpioPin dataPin) : base(dataPin) { }
    
        public override DHTData ReadData()
        {
            Read();
            float t = _data[2];
            float h = _data[0];

            return new DHTData(t, h);
        }
    }
}