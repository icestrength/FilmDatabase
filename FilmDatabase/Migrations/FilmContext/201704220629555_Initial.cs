namespace FilmDatabase.Migrations.FilmContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        MarkValue = c.Int(nullable: false),
                        FilmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true)
                .Index(t => t.FilmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Marks", "FilmId", "dbo.Films");
            DropIndex("dbo.Marks", new[] { "FilmId" });
            DropTable("dbo.Marks");
        }
    }
}
