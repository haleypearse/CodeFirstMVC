namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.People", "LastMet", c => c.DateTime());
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.People", "WhenMet", c => c.DateTime());
           // AddPrimaryKey("dbo.People", "Name");
           // DropColumn("dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PersonId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "WhenMet", c => c.DateTime(nullable: false));
            AlterColumn("dbo.People", "Name", c => c.String());
            DropColumn("dbo.People", "LastMet");
        }
    }
}
