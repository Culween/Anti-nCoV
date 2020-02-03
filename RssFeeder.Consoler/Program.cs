using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RssFeeder.FeedCore;
using System;
using System.Collections.Generic;
using System.Data;
using RssFeeder.Storage.Services;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using RssFeeder.Storage.Models;

namespace RssFeeder.Consoler
{
    class Program
    {
        //static List<WeiboRssFeedItem> RssFeeds = new List<WeiboRssFeedItem>
        //{
        //    new WeiboRssFeedItem(){
        //        Url="https://rssfeed.today/weibo/rss/1770713970",
        //        Name="长沙市疾控中心",
        //        Province="湖南省",
        //        City="长沙"
        //    }
        //};

        public static IConfiguration Configuration { get; set; }

        static string ConnectionString = "mongodb://localhost:27017";
        static string DatabaseName = "nCoVData";

        static List<string> WeiboNameKeywords = new List<string>
        {
            "发布","报","新闻","疫","卫","健"
        };

        static List<string> Provinces = new List<string>
        {
            "北京市","天津市","河北省","山西省","内蒙古自治区", "辽宁省","吉林省","黑龙江省","上海市","江苏省","浙江省","安徽省","福建省","江西省","山东省","河南省","湖北省","湖南省","广东省","广西壮族自治区","海南省","重庆市","四川省","贵州省","云南省","西藏自治区",  "陕西省","甘肃省","青海省","宁夏回族自治区","新疆维吾尔自治区"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ImportWeiboUserRss();

            Console.Read();
        }

        static void ImportCityData()
        {
            var myJsonString = File.ReadAllText("city.json");
            var myJObject = JObject.Parse(myJsonString);
            var addressService = new CityService(ConnectionString, DatabaseName);

            foreach (var item in myJObject.Children().Children().Children())
            {
                string province = item["province"].Value<string>();
                string city = item["name"].Value<string>();
                string code = item["id"].Value<string>();
                if (addressService.GetByCityName(city) != null) continue;

                addressService.Create(new Storage.Models.CityItem()
                {
                    Province = province,
                    City = city,
                    Nation = "中国",
                    Code = code,
                });
            }
        }

        static void ImportWeiboUserData()
        {

            DataTable tb = ExcelHelper.ReadExcelToDataTable(@"D:\临时\WeiboUsers.xlsx");


            var service = new RssFeedService(ConnectionString, DatabaseName);
            var cityService = new CityService(ConnectionString, DatabaseName);
            var userService = new WeiboUserService(ConnectionString, DatabaseName);
            List<DataRow> rows = new List<DataRow>();
            DataTable filtedDt = tb.Clone();
            filtedDt.Clear();

            foreach (DataRow row in tb.Rows)
            {
                string vipStyle = row["官方认证样式"]?.ToString();
                if (string.IsNullOrEmpty(vipStyle) || !vipStyle.Contains("icon-vip-b"))
                    continue;

                string name = row["名称"]?.ToString();
                if (string.IsNullOrEmpty(name))
                    continue;

                if (WeiboNameKeywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    filtedDt.ImportRow(row);
                }
                else
                {
                    continue;
                }

                WeiboUser user = new WeiboUser()
                {
                    Name = name,
                    Url = row["微博地址"]?.ToString(),
                    Address = row["省"]?.ToString(),
                    Description = row["描述"]?.ToString(),
                    Followers = row["粉丝"]?.ToString(),
                    PublishCount = row["微博数"]?.ToString(),
                    Tags = row["标签"]?.ToString(),
                    Summary = row["简介"]?.ToString(),
                    UpdateTime = DateTime.Now
                };

                if (string.IsNullOrEmpty(user.Address) && string.IsNullOrEmpty(user.Description))
                    continue;

                string province = Provinces.FirstOrDefault(k => user.Description.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
                string city = "";

                if (!string.IsNullOrEmpty(user.Address))
                {
                    var cityData = cityService.GetLikeCityName(user.Address);
                    if (cityData != null)
                    {
                        city = cityData.City;
                        if (province == null)
                        {
                            province = cityData.Province;
                        }
                    }
                }

                if (string.IsNullOrEmpty(province) && string.IsNullOrEmpty(city))
                    continue;

                user.Province = province;
                user.City = city;
                userService.Create(user);
            }
        }

        static void ImportWeiboUserRss()
        {
            var userService = new WeiboUserService(ConnectionString, DatabaseName);
            DataTable tb = ExcelHelper.ReadExcelToDataTable(@"F:\Development\wuhan2020\544WeiboUsersRSS.xlsx");

            foreach (DataRow row in tb.Rows)
            {
                string name = row["名称"]?.ToString();

                WeiboUser user = userService.GetByName(name);
                if (user != null)
                {
                    user.UserId = row["userId"]?.ToString();
                    user.UserIdUrl = row["UserId地址"]?.ToString();
                    user.RssUrl = row["RSS地址"]?.ToString();

                    userService.Update(user.Id, user);
                }
            }
        }

        static void Read()
        {
            //foreach (var item in RssFeeds)
            //{
            //    RssParser.Parse(item.Url);
            //}
        }
    }
}
