namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHumanizedDatesToPersonModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "HumanizedWhenMet", c => c.String());
            AddColumn("dbo.People", "HumanizedLastMet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "HumanizedLastMet");
            DropColumn("dbo.People", "HumanizedWhenMet");
        }
    }
}
