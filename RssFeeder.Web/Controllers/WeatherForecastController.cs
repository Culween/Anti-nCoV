using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RssFeeder.FeedCore;
using RssFeeder.Storage.Models;
using RssFeeder.Storage.Services;

namespace RssFeeder.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        static List<RssFeed> RssFeeds = new List<RssFeed>
        {
            new RssFeed(){
                Url="https://rssfeed.today/weibo/rss/1770713970",
                Name="长沙市疾控中心",
                Province="湖南省",
                City="长沙"
            }
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly RssDataService _rssDataService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, RssDataService rssDataService)
        {
            _logger = logger;
            _rssDataService = rssDataService;

            foreach (var feed in RssFeeds)
            {
                IList<RssData> rssDatas = RssParser.Parse(feed.Url);
                foreach (var data in rssDatas)
                {
                    if (_rssDataService.GetByTitle(data.Title) != null)
                    {
                        continue;
                    }
                    data.City = feed.City;
                    data.Province = feed.Province;
                    data.RssFeedName = feed.Name;
                    _rssDataService.Create(data);
                }
            }
        }

        [HttpGet]
        public IEnumerable<RssData> Get()
        {
            //var rng = new Random();

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            List<RssData> rssDatas = _rssDataService.Get();
            return rssDatas;
        }
    }
}
