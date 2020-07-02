using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    public sealed class DHT21 : DHT22
    {
        public DHT21(GpioPin dataPin) : base(dataPin) { }
    }
}