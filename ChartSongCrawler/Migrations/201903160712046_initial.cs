namespace ChartSongCrawler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CrawlingTargetSongs",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        Title = c.String(maxLength: 1000),
                        Singer = c.String(maxLength: 1000),
                        Year = c.Int(nullable: false),
                        Lyric = c.String(storeType: "ntext"),
                        Status = c.String(maxLength: 20),
                        Message = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.SongId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CrawlingTargetSongs");
        }
    }
}
