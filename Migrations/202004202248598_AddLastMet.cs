namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastMet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "LastMet", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "LastMet");
        }
    }
}
