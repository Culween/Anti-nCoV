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

        static List<WeiboUser> RssFeeds = new List<WeiboUser> { };
        static Queue<WeiboUser> RssFeedsQueue = new Queue<WeiboUser>();

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly NewsService _rssDataService;
        private readonly WeiboUserService _weiboUserService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, NewsService rssDataService, WeiboUserService weiboUserService)
        {
            _logger = logger;
            _rssDataService = rssDataService;
            _weiboUserService = weiboUserService;

            RssFeeds = _weiboUserService.Get();

            foreach (var feed in RssFeeds)
            {
                RssFeedsQueue.Enqueue(feed);

            }

            Parallel.For(0, 9, i =>
             {
                 int count = 0;
                 while (GetRssNews(i))
                 {
                     count++;
                     Console.WriteLine($"Thread[{i}] get rss times: {count}");
                     _logger.LogInformation($"Thread[{i}] get rss times: {count}");
                 }
             });
        }

        bool GetRssNews(int id)
        {
            WeiboUser user = null;
            try
            {
                if (RssFeedsQueue.Count == 0)
                    return false;

                user = RssFeedsQueue.Dequeue();
                if (user == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Thread[{id}] error.", ex);
                return false;
            }

            try
            {
                IList<News> newsDatas = RssParser.Parse(user.RssUrl);
                foreach (var news in newsDatas)
                {
                    try
                    {
                        if (_rssDataService.GetByTitle(news.Title) != null)
                        {
                            continue;
                        }

                        news.City = user.City;
                        news.Country = user.Country;
                        news.Province = user.Province;
                        news.Publisher = user.Name;
                        news.Source = "微博";
                        news.Status = NewsStatus.Unconfirmed;
                        _rssDataService.Create(news);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Thread[{id}] mongo error.", ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Thread[{id}] rss error.", ex);
            }

            return true;
        }


        [HttpGet]
        public IEnumerable<News> Get()
        {
            //var rng = new Random();

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            List<News> rssDatas = _rssDataService.Get();
            return rssDatas;
        }
    }
}
