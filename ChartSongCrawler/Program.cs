using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ChartSongCrawler
{
    public class Program
    {
        public static ILog log = LogManager.GetLogger("Program");

        static void Main(string[] args)
        {
            log.Debug("크롤링 시작....");

            var crawler = new ChartSongCrawling();
            crawler.Start();

            log.Debug("크롤링 종료");
        }
    }
}
