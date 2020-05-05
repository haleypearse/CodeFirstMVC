namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMeetingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Person_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Name)
                .Index(t => t.Person_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meetings", "Person_Name", "dbo.People");
            DropIndex("dbo.Meetings", new[] { "Person_Name" });
            DropTable("dbo.Meetings");
        }
    }
}
