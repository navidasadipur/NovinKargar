namespace NovinKargar.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addServiceModelsAndAddToMyDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 400),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 600),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        ViewCount = c.Int(nullable: false),
                        Image = c.String(),
                        AddedDate = c.DateTime(),
                        ServiceCategoryId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCategories", t => t.ServiceCategoryId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ServiceCategoryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ServiceHeadLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 700),
                        Description = c.String(),
                        ServiceId = c.Int(nullable: false),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.ServiceOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedDate = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(),
                        CustomerId = c.Int(),
                        CustomerFirstName = c.String(maxLength: 500),
                        CustomerLastName = c.String(maxLength: 500),
                        GeoDivisionId = c.Int(),
                        Address = c.String(maxLength: 500),
                        Phone = c.String(maxLength: 50),
                        PostalCode = c.String(maxLength: 50),
                        Email = c.String(),
                        Description = c.String(),
                        ServiceId = c.Int(nullable: false),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.GeoDivisions", t => t.GeoDivisionId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.GeoDivisionId)
                .Index(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServiceOrders", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceOrders", "GeoDivisionId", "dbo.GeoDivisions");
            DropForeignKey("dbo.ServiceOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ServiceHeadLines", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories");
            DropIndex("dbo.ServiceOrders", new[] { "ServiceId" });
            DropIndex("dbo.ServiceOrders", new[] { "GeoDivisionId" });
            DropIndex("dbo.ServiceOrders", new[] { "CustomerId" });
            DropIndex("dbo.ServiceHeadLines", new[] { "ServiceId" });
            DropIndex("dbo.Services", new[] { "UserId" });
            DropIndex("dbo.Services", new[] { "ServiceCategoryId" });
            DropTable("dbo.ServiceOrders");
            DropTable("dbo.ServiceHeadLines");
            DropTable("dbo.Services");
            DropTable("dbo.ServiceCategories");
        }
    }
}
