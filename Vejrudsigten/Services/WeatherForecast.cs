using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejrudsigten.Services
{
    public class WeatherForecast
    {
        private const string Location = "Kolding"; 
        private readonly IWeatherService _weatherService = null;

        public WeatherForecast(IWeatherService weatherService = null)
        {
            this._weatherService = weatherService ?? new WeatherService();
        }

        public async Task<string> GetForecastAsync(string key)
        {
            var weatherInfo = await GetWeatherInfo(key);
            var todayInfo = weatherInfo.Item1 as WeatherInfo;
            var yesterdayInfo = weatherInfo.Item2 as WeatherInfo;
            String result = "Vejret i Kolding er {0} og der er {1} grader. I går var det {2} og {3} grader";
            return String.Format(result, todayInfo.Conditions, todayInfo.Temperature, yesterdayInfo.Conditions, yesterdayInfo.Temperature);
        }

        private async Task<Tuple<WeatherInfo, WeatherInfo>> GetWeatherInfo(string key)
        {
            var todayInfo = await _weatherService.GetTodaysWeather(key, Location);
            var yesterdayInfo = await _weatherService.GetYesterdaysWeather(key, Location);
            return new Tuple<WeatherInfo, WeatherInfo>(todayInfo, yesterdayInfo);
        }

        public async Task<string> GetForecastTitleAsync(string key)
        {
            var weatherInfo = await GetWeatherInfo(key);
            WeatherInfo todayInfo = weatherInfo.Item1;
            WeatherInfo yesterdayInfo = weatherInfo.Item2;

            string title = WeatherTitleConst.TypicalWeather;

            if (yesterdayInfo.Conditions.Equals(WeatherConditionConst.Rain) && todayInfo.Conditions.Equals(WeatherConditionConst.Rain))
                title = WeatherTitleConst.RainRain;
            else if (yesterdayInfo.Conditions.Equals(WeatherConditionConst.Clear) && todayInfo.Conditions.Equals(WeatherConditionConst.Rain))
                title = WeatherTitleConst.ClearRain;
            else if (yesterdayInfo.Conditions.Equals(WeatherConditionConst.Clear) && todayInfo.Conditions.Equals(WeatherConditionConst.Clear) 
                     && yesterdayInfo.Temperature < 25 && todayInfo.Temperature < 25)
                title = WeatherTitleConst.ClearClearGood;
            else if (yesterdayInfo.Conditions.Equals(WeatherConditionConst.Clear) && todayInfo.Conditions.Equals(WeatherConditionConst.Clear) 
                     && yesterdayInfo.Temperature >= 25 && todayInfo.Temperature >= 25)
                title = WeatherTitleConst.ClearClearHot;
            else if (!yesterdayInfo.Conditions.Equals(WeatherConditionConst.Snow) && todayInfo.Conditions.Equals(WeatherConditionConst.Snow))
                title = WeatherTitleConst.NotSnowSnow;
            else if (yesterdayInfo.Conditions.Equals(WeatherConditionConst.Snow) && todayInfo.Conditions.Equals(WeatherConditionConst.Snow))
                title = WeatherTitleConst.SnowSnow;
            else if (yesterdayInfo.Temperature >= 10 && todayInfo.Temperature < 10)
                title = WeatherTitleConst.ColdWeatherComing;
            return title;
        }
    }
}
