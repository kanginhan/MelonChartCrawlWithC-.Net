using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartSongCrawler.Model
{
    public class CrawlingTargetSong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SongId { get; set; }
        [Column(TypeName ="NVARCHAR")]
        [MaxLength(1000)]
        public string Title { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(1000)]
        public string Singer { get; set; }
        public int Year { get; set; }
        [Column(TypeName = "NTEXT")]
        public string Lyric { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}
