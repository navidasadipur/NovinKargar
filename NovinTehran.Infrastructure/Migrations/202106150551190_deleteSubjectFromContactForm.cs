namespace NovinTehran.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteSubjectFromContactForm : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactForms", "Subject", c => c.String(maxLength: 600));
        }
    }
}
