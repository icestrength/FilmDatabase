namespace FilmDatabase.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Her");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Her", c => c.String());
        }
    }
}
