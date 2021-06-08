namespace NovinTehran.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentToArticleCategoryModel : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Offers", "Description", c => c.String());
            //AddColumn("dbo.Invoices", "CompanyName", c => c.String());
            //AddColumn("dbo.Invoices", "Country", c => c.String());
            //AddColumn("dbo.Invoices", "City", c => c.String());
            //AddColumn("dbo.Invoices", "Description", c => c.String());
            AddColumn("dbo.ArticleCategories", "ParentId", c => c.Int());
            //AddColumn("dbo.ContactForms", "Subject", c => c.String(maxLength: 600));
            CreateIndex("dbo.ArticleCategories", "ParentId");
            AddForeignKey("dbo.ArticleCategories", "ParentId", "dbo.ArticleCategories", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ArticleCategories", "ParentId", "dbo.ArticleCategories");
            //DropIndex("dbo.ArticleCategories", new[] { "ParentId" });
            //DropColumn("dbo.ContactForms", "Subject");
            //DropColumn("dbo.ArticleCategories", "ParentId");
            //DropColumn("dbo.Invoices", "Description");
            //DropColumn("dbo.Invoices", "City");
            //DropColumn("dbo.Invoices", "Country");
            //DropColumn("dbo.Invoices", "CompanyName");
            //DropColumn("dbo.Offers", "Description");
        }
    }
}
