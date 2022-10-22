using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejrudsigten.Services;

namespace Vejrudsigten.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly WeatherForecast _weatherForecast;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _weatherForecast = new WeatherForecast();
        }

        public async Task OnGetAsync()
        {
            var key = _configuration["key"];

            if (key == null)
            {
                ViewData.Add("Vejrudsigten", "Hov! Du har glemt at angive nøglen i appsettings.local.json. Gå tilbage til opgavebeskrivelsen og se hvordan");
            } else
            {
                ViewData.Add("WeatherTitle", await _weatherForecast.GetForecastTitleAsync(key));
                ViewData.Add("Vejrudsigten", await _weatherForecast.GetForecastAsync(key));
            }
        }
    }
}
