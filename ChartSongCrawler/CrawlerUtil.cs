using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartSongCrawler
{
    public static class CrawlerUtil
    {
        public static CrawlConfiguration config = new CrawlConfiguration {
            CrawlTimeoutSeconds = 0,
            DownloadableContentTypes = "text/html, text/plain",
            HttpServicePointConnectionLimit = 200,
            HttpRequestTimeoutInSeconds = 35,
            HttpRequestMaxAutoRedirects = 7,
            IsExternalPageCrawlingEnabled = false,
            IsExternalPageLinksCrawlingEnabled = false,
            IsUriRecrawlingEnabled = false,
            IsHttpRequestAutoRedirectsEnabled = true,
            IsHttpRequestAutomaticDecompressionEnabled = false,
            IsRespectRobotsDotTextEnabled = false,
            IsRespectMetaRobotsNoFollowEnabled = false,
            IsRespectAnchorRelNoFollowEnabled = false,
            IsForcedLinkParsingEnabled = false,
            /* activate */
            IsSendingCookiesEnabled = true,
            MaxConcurrentThreads = 10,
            MaxPagesToCrawl = 1000,
            MaxPagesToCrawlPerDomain = 0,
            MaxPageSizeInBytes = 0,
            MaxMemoryUsageInMb = 0,
            MaxMemoryUsageCacheTimeInSeconds = 0,
            MaxRobotsDotTextCrawlDelayInSeconds = 5,
            MaxCrawlDepth = 0,
            MinAvailableMemoryRequiredInMb = 0,
            MinCrawlDelayPerDomainMilliSeconds = 1000,
            UserAgentString = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko"
        };

        public static string GetPCID()
        {
            var pcid = DateTime.Now.vGetTime().ToString();
            for (var i = 0; i < 10; i++) {
                var ran = new Random();
                pcid += ran.Next(0, 9).ToString();
            }
            return pcid;
        }

        public static long vGetTime(this DateTime dateTime)
        {
            long retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (dateTime.ToUniversalTime() - st);
            retval = (long)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        public static string vSetMonParam(this object source)
        {
            return source.ToString().PadLeft(2, '0');
        }

        public static string GetClassCd(int year, int mon)
        {
            if (year < 2004 || (year == 2004 && mon < 11)) {
                return "KPOP";
            }
            else if (year < 2017) {
                return "DP0100";
            }
            else {
                return "DM0000";
            }
        }
    }
}
