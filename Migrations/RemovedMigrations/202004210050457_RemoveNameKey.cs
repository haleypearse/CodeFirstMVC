namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNameKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "Name", c => c.String());
            AlterColumn("dbo.People", "PersonId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "PersonId", c => c.Int(nullable: false));
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.People", "Name");
        }
    }
}
