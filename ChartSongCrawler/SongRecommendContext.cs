using ChartSongCrawler.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartSongCrawler
{
    public class SongRecommendContext : DbContext
    {
        public SongRecommendContext()
            : base("name=SongRecommend")
        {
        }

        public DbSet<CrawlingTargetSong> CrawlingTargetSong { get; set; }
    }
}
