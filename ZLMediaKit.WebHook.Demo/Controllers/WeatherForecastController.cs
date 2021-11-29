using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZLMediaKit.Common.Dtos.ApiInputDto;
//using ZLMediaKit.HttpApi.Dtos;

namespace ZLMediaKit.WebHook.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ZLMediaKit.HttpApi.ZLHttpClient _zLHttpClient;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpApi.ZLHttpClient zLHttpClient)
        {
            _logger = logger;
            this._zLHttpClient = zLHttpClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetStatistic")]
        public async Task<IApiGetStatisticResult> GetStatistic()
        {
            var value = await _zLHttpClient.getStatistic();
            return value;
        }

        [HttpGet("GetConfig")]
        public async Task<IApiGetServerConfigResult> GetConfig()
        {
            return await _zLHttpClient.GetServerConfig();
        }

        [HttpGet("GetBin")]
        public IActionResult GetdownloadBin()
        {
            return File(_zLHttpClient.DownloadBin().Result, "application/octet-stream", "MediaServer.exe");
        }
    }
}
