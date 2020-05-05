namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLuckyNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "LuckyNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "LuckyNumber");
        }
    }
}
