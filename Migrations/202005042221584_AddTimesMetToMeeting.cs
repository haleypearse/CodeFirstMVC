namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimesMetToMeeting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "TimesMet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meetings", "TimesMet");
        }
    }
}
