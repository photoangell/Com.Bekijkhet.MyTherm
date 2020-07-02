using System;
using System.Diagnostics;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Com.Bekijkhet.MyTherm
{
    public abstract class DHT
    {
        protected UInt32[] _data;
        private GpioPin _dataPin;
        private bool _firstReading;
        private DateTime _prevReading;

        public DHT(GpioPin dataPin)
        {
            if (dataPin == null)
                throw new ArgumentException("Parameter cannot be null.", "dataPin");

            _dataPin = dataPin;
            _firstReading = true;
            _prevReading = DateTime.MinValue;
            _data = new UInt32[6];
            //Init the data pin
            _dataPin.PinMode = GpioPinDriveMode.Output;
            _dataPin.Write(GpioPinValue.High);
        }

        public abstract DHTData ReadData();

        public DHTData ReadDataWithRetries() {
            for (var i = 1; i <= 15; i++) {
                try {
                    var data = ReadData();
                    return data;
                } 
                catch (Exception e) {
                    Debug.WriteLine (e.Message);
                }
                Thread.Sleep(2000);
            }
            throw new DHTException("read retries exceeded", new IndexOutOfRangeException());
        }

        protected void Read()
        {
            var now = DateTime.UtcNow;

            if (!_firstReading && ((now - _prevReading).TotalMilliseconds < 2000))
                throw new DHTException("attempted re read too soon", new InvalidOperationException());

            _firstReading = false;
            _prevReading = now;

            _data[0] = _data[1] = _data[2] = _data[3] = _data[4] = 0;

            _dataPin.PinMode = GpioPinDriveMode.Output;
            _dataPin.Write(GpioPinValue.High);
            Thread.Sleep(500);
            _dataPin.Write(GpioPinValue.Low);
            Thread.Sleep(20);

            //TIME CRITICAL ###############
            _dataPin.Write(GpioPinValue.High);
            //=> DELAY OF 40 microseconds needed here
            WaitMicroseconds(40);

            _dataPin.PinMode = GpioPinDriveMode.Input;
            //Delay of 10 microseconds needed here
            WaitMicroseconds(10);

            if (ExpectPulse(GpioPinValue.Low) == 0)
                throw new DHTException("expected low pulse failed", new InvalidOperationException());

            if (ExpectPulse(GpioPinValue.High) == 0)
                throw new DHTException("expected high pulse failed", new InvalidOperationException());

            // Now read the 40 bits sent by the sensor.  Each bit is sent as a 50
            // microsecond low pulse followed by a variable length high pulse.  If the
            // high pulse is ~28 microseconds then it's a 0 and if it's ~70 microseconds
            // then it's a 1.  We measure the cycle count of the initial 50us low pulse
            // and use that to compare to the cycle count of the high pulse to determine
            // if the bit is a 0 (high state cycle count < low state cycle count), or a
            // 1 (high state cycle count > low state cycle count).
            for (int i = 0; i < 40; ++i)
            {
                UInt32 lowCycles = ExpectPulse(GpioPinValue.Low);
                if (lowCycles == 0)
                    throw new DHTException("expected low cycles failed", new InvalidOperationException());

                UInt32 highCycles = ExpectPulse(GpioPinValue.High);
                if (highCycles == 0)
                    throw new DHTException("expected high cycles failed", new InvalidOperationException());

                _data[i / 8] <<= 1;
                // Now compare the low and high cycle times to see if the bit is a 0 or 1.
                if (highCycles > lowCycles)
                {
                    // High cycles are greater than 50us low cycle count, must be a 1.
                    _data[i / 8] |= 1;
                }
                // Else high cycles are less than (or equal to, a weird case) the 50us low
                // cycle count so this must be a zero.  Nothing needs to be changed in the
                // stored data.
            }
            //TIME CRITICAL_END #############

            // Check we read 40 bits and that the checksum matches.
            if (_data[4] != ((_data[0] + _data[1] + _data[2] + _data[3]) & 0xFF))
                throw new DHTException("checksum failed", new InvalidOperationException());
        }

        private UInt32 ExpectPulse(GpioPinValue level)
        {
            UInt32 count = 0;

            while (_dataPin.Read() == (level == GpioPinValue.High))
            {
                count++;
                //WaitMicroseconds(1);
                if (count == 32000)
                {
                    return 0;
                }
            }
            return count;
        }

        private void OldWaitMicroseconds(int microseconds)
        {
            var until = DateTime.UtcNow.Ticks + (microseconds * 10) - 20;
            while (DateTime.UtcNow.Ticks < until) { }
        }

        private void WaitMicroseconds(int microseconds)
        {
            var durationTicks = (long)Math.Round((decimal)((Stopwatch.Frequency / 1000000) * microseconds));
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks - 1600) { }
        }
    }
}