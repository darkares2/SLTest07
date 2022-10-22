using System.Threading.Tasks;
using Moq;
using Vejrudsigten.Services;
using Xunit;

namespace VejrudsigtenTest;

public class UnitTestWeatherForecast
{
    [Theory]
    [InlineData(WeatherConditionConst.Rain, WeatherConditionConst.Rain, 10.0, 10.0, WeatherTitleConst.RainRain)]
    [InlineData(WeatherConditionConst.Clear, WeatherConditionConst.Rain, 10.0, 10.0, WeatherTitleConst.ClearRain)]
    [InlineData(WeatherConditionConst.Clear, WeatherConditionConst.Clear, 24.9, 24.9, WeatherTitleConst.ClearClearGood)]
    [InlineData(WeatherConditionConst.Clear, WeatherConditionConst.Clear, 25.0, 25.0, WeatherTitleConst.ClearClearHot)]
    [InlineData(WeatherConditionConst.Clear, WeatherConditionConst.Snow, 25.0, 25.0, WeatherTitleConst.NotSnowSnow)]
    [InlineData(WeatherConditionConst.Snow, WeatherConditionConst.Snow, 25.0, 25.0, WeatherTitleConst.SnowSnow)]
    [InlineData(WeatherConditionConst.Snow, WeatherConditionConst.Other, 10.0, 9.9, WeatherTitleConst.ColdWeatherComing)]
    [InlineData(WeatherConditionConst.Other, WeatherConditionConst.Clear, 10.0, 9.9, WeatherTitleConst.ColdWeatherComing)]
    [InlineData(WeatherConditionConst.Rain, WeatherConditionConst.Rain, 10.0, 9.9, WeatherTitleConst.RainRain)]
    [InlineData(WeatherConditionConst.Other, WeatherConditionConst.Rain, 5.0, 9.9, WeatherTitleConst.TypicalWeather)]
    public async Task Test1(string yesterdayCondition, string todayCondition, double yesterdayTemp, double todayTemp, string expectedResult)
    {
        var sut = SetupSut(yesterdayCondition, todayCondition, yesterdayTemp, todayTemp);

        var result = await sut.GetForecastTitleAsync("key");
        
        Assert.Equal(expectedResult, result);
    }

    private WeatherForecast SetupSut(string yesterdayCondition, string todayCondition, double yesterdayTemp, double todayTemp)
    {
        var weatherServiceMock = new Mock<IWeatherService>();
        weatherServiceMock.Setup(x => x.GetTodaysWeather(It.IsAny<string>(), It.IsAny<string>())).Returns( Task.FromResult(new WeatherInfo()
        {
            Conditions = todayCondition, Temperature = todayTemp
        }));
        weatherServiceMock.Setup(x => x.GetYesterdaysWeather(It.IsAny<string>(), It.IsAny<string>())).Returns( Task.FromResult(new WeatherInfo()
        {
            Conditions = yesterdayCondition, Temperature = yesterdayTemp
        }));

        return new WeatherForecast(weatherServiceMock.Object);
    }
}