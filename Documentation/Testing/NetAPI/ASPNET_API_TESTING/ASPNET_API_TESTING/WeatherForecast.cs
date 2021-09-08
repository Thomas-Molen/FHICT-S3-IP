using System;

namespace ASPNET_API_TESTING
{
    public class WeatherForecast
    {
        /// <summary>
        ///  Date for wheather
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Tempature in C
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in F
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Summary
        /// </summary>
        public string Summary { get; set; }
    }
}
