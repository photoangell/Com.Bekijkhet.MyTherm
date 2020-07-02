using System;

namespace Com.Bekijkhet.MyTherm
{
    public static class TemperatureUtils
    {
        public static float ConvertCtoF(float c)
        {
            return c * 9 / 5 + 32;
        }

        public static float ConvertFtoC(float f)
        {
            return (f - 32) * 5 / 9;
        }

        public static double ComputeHeatIndexFromFahreneit(float temperature, float percentHumidity)
        {
            return -42.379 +
                2.04901523 * temperature +
                10.14333127 * percentHumidity +
                -0.22475541 * temperature * percentHumidity +
                -0.00683783 * Math.Pow(temperature, 2) +
                -0.05481717 * Math.Pow(percentHumidity, 2) +
                0.00122874 * Math.Pow(temperature, 2) * percentHumidity +
                0.00085282 * temperature * Math.Pow(percentHumidity, 2) +
                -0.00000199 * Math.Pow(temperature, 2) * Math.Pow(percentHumidity, 2);
        }

        public static double ComputeHeatIndexFromCelsius(float temperature, float percentHumidity)
        {
            return -8.784695 +
                1.61139411 * temperature +
                2.338549 * percentHumidity +
                -0.14611605 * temperature * percentHumidity +
                -0.01230809 * Math.Pow(temperature, 2) +
                -0.01642482 * Math.Pow(percentHumidity, 2) +
                0.00221173 * Math.Pow(temperature, 2) * percentHumidity +
                0.00072546 * temperature * Math.Pow(percentHumidity, 2) +
                -0.00000358 * Math.Pow(temperature, 2) * Math.Pow(percentHumidity, 2);
        }
    }
}