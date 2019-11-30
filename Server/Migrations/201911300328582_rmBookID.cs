namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmBookID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Books");
            AlterColumn("dbo.Books", "BookName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Books", "BookName");
            DropColumn("dbo.Books", "BookId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "BookId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Books");
            AlterColumn("dbo.Books", "BookName", c => c.String());
            AddPrimaryKey("dbo.Books", "BookId");
        }
    }
}
