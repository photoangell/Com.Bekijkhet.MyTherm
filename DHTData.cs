namespace Com.Bekijkhet.MyTherm
{
    public class DHTData
    {
        public float TempCelsius {get; set;}
        public float TempFahrenheit {get; set;}
        public float Humidity {get; set;}
        public double HeatIndex {get;set;}    

        public DHTData(float t, float h)
        {
            TempCelsius = t;
            TempFahrenheit = TemperatureUtils.ConvertCtoF(t);
            Humidity = h;
            HeatIndex = TemperatureUtils.ComputeHeatIndexFromCelsius(t, h);
        }
    }
}