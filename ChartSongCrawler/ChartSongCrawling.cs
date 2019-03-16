using Abot.Crawler;
using Abot.Poco;
using ChartSongCrawler.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChartSongCrawler
{
    public class ChartSongCrawling
    {
        /// <summary>
        /// 차트 크롤링 실패횟수
        /// </summary>
        private int _failtCount = 0;

        public void Start()
        {
            //---------------------------
            // 검색 차트 설정
            //---------------------------
            for (var age = 1990; age <= 2010; age += 10) {
                for (var year = age; year <= age + 9; year++) {
                    for (var mon = 1; mon <= 12; mon++) {
                        Program.log.Debug($"{year}년 {mon}월 차트 크롤링을 시작합니다...");

                        //---------------------------
                        // 종료조건
                        //---------------------------
                        if (_failtCount >= 10) {
                            Program.log.Debug($"크롤링 실패횟수가 10회 초과하였습니다. 프로그램을 종료합니다");
                            return;
                        }

                        //---------------------------
                        // 크롤링 설정
                        //---------------------------
                        var cookieContainer = new CookieContainer();
                        cookieContainer.Add(new Cookie("PCID", CrawlerUtil.GetPCID(), "/", ".melon.com"));
                        var pageRequester = new PageRequester(CrawlerUtil.config, cookieContainer);
                        var crawler = new PoliteWebCrawler(CrawlerUtil.config, null, null, null, pageRequester, null, null, null, null);
                        crawler.PageCrawlCompletedAsync += ProcessPageCrawlCompletedAsync;

                        //---------------------------
                        // 크롤링 시작
                        //---------------------------
                        var classCd = CrawlerUtil.GetClassCd(year, mon);
                        crawler.CrawlBag = new { year };
                        crawler.Crawl(new Uri($"https://www.melon.com/chart/search/list.htm?chartType=MO&age={age}&year={year}&mon={mon.vSetMonParam()}&classCd={classCd}&moved=Y"));

                    }
                }
            }
        }

        private void ProcessPageCrawlCompletedAsync(object sender, PageCrawlCompletedArgs e)
        {
            var crawledPage = e.CrawledPage;
            var doc = crawledPage.HtmlDocument.DocumentNode;
            var songNodes = doc.SelectNodes("//tr[@class='lst50'] | //tr[@class='lst100']");

            //---------------------------
            // 크롤링 유효성 검사
            //---------------------------
            if (songNodes == null || songNodes.Count == 0) {
                Program.log.Debug($"멜론차트 크롤링 실패: 자료가 없습니다");
                _failtCount++;
                return;
            }

            Program.log.Debug($"멜론차트 크롤링 성공: {songNodes.Count} 건");
            foreach (var node in songNodes) {
                try {
                    using (var db = new SongRecommendContext()) {
                        //---------------------------
                        // 노래정보 파싱
                        //---------------------------
                        var songId = node.SelectSingleNode(".//input[@class='input_check']").GetAttributeValue("value", 0);
                        var title = node.SelectSingleNode(".//div[@class='ellipsis rank01']//a | .//div[@class='ellipsis rank01']//span[@class='fc_lgray']").InnerText;
                        var singer = node.SelectSingleNode(".//div[@class='ellipsis rank02']").LastChild.InnerText;
                        if (songId == 0 || db.CrawlingTargetSong.Find(songId) != null) {
                            continue;
                        }

                        //---------------------------
                        // 가사 가져오기
                        //---------------------------
                        Program.log.Debug($"{songId} {singer} {title} 가사 가져오기...");
                        HttpClient client = new HttpClient();
                        string jsonString = client.GetStringAsync($"https://www.melon.com/song/lyricInfo.json?songId={songId}").Result;
                        var lyric = JObject.Parse(jsonString).Value<string>("lyric");
                        if (lyric == null || lyric.Length == 0) {
                            Program.log.Debug($"{songId} {singer} {title} 실패: 가사가 없습니다");
                            continue;
                        }

                        //---------------------------
                        // DB 저장
                        //---------------------------
                        Program.log.Debug($"{songId} {singer} {title} DB 저장....");
                        db.CrawlingTargetSong.Add(new CrawlingTargetSong {
                            SongId = songId,
                            Title = title,
                            Singer = singer,
                            Year = ((PoliteWebCrawler)sender).CrawlBag.year,
                            Lyric = Regex.Replace(lyric, @"<br>|<br/>|</br>", " ", RegexOptions.IgnoreCase).Trim(),
                            Status = CrawlingStatus.Ready
                        });
                        db.SaveChanges();
                        Program.log.Debug($"{songId} {singer} {title} DB 저장 완료");
                    }
                }
                catch (Exception ex) {
                    Program.log.Debug($"에러 발생: {ex.Message}");
                }
            }
        }
    }
}
