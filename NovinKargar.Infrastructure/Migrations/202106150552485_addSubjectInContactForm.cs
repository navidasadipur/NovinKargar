namespace NovinKargar.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubjectInContactForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactForms", "Subject", c => c.String(maxLength: 600));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactForms", "Subject");
        }
    }
}
