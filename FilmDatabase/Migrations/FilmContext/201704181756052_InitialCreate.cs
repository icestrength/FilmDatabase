namespace FilmDatabase.Migrations.FilmContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Date = c.DateTime(nullable: false),
                        Body = c.String(nullable: false),
                        FilmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true)
                .Index(t => t.FilmId);
            
            CreateTable(
                "dbo.CategoryFilm",
                c => new
                    {
                        FilmId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilmId, t.CategoryId })
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.FilmId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "FilmId", "dbo.Films");
            DropForeignKey("dbo.CategoryFilm", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryFilm", "FilmId", "dbo.Films");
            DropIndex("dbo.Comments", new[] { "FilmId" });
            DropIndex("dbo.CategoryFilm", new[] { "CategoryId" });
            DropIndex("dbo.CategoryFilm", new[] { "FilmId" });
            DropTable("dbo.CategoryFilm");
            DropTable("dbo.Comments");
            DropTable("dbo.Films");
            DropTable("dbo.Categories");
        }
    }
}
