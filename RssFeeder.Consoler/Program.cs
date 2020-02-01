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

        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            Console.WriteLine("Excel path：");

            //Console.WriteLine($"section0:UserId:{configuration["section0:UserId"]}");
            string path = Console.ReadLine();

            DataTable tb = ExcelHelper.ReadExcelToDataTable(@"D:\临时\WeiboUsers.xlsx");


            var service = new RssFeedService(ConnectionString, DatabaseName);
            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in tb.Rows)
            {
                string name = row["名称"]?.ToString();
                //name.
                //if (row["名称"])
                //{

                //}
            }


            Console.Read();

        }

        static void ImoprtCityData()
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

        static void Read()
        {
            //foreach (var item in RssFeeds)
            //{
            //    RssParser.Parse(item.Url);
            //}
        }
    }
}
