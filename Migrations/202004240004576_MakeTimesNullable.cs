namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "WhenMet", c => c.DateTime());
            AlterColumn("dbo.People", "LastMet", c => c.DateTime());
            DropColumn("dbo.People", "LuckyNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "LuckyNumber", c => c.String());
            AlterColumn("dbo.People", "LastMet", c => c.DateTime(nullable: false));
            AlterColumn("dbo.People", "WhenMet", c => c.DateTime(nullable: false));
        }
    }
}
