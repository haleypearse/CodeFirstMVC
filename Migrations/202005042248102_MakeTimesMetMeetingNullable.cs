namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTimesMetMeetingNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Meetings", "TimesMet", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meetings", "TimesMet", c => c.Int(nullable: false));
        }
    }
}
