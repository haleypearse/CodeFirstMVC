namespace CodeFirstMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PersonId", c => c.Int(nullable: false));
        }
    }
}
