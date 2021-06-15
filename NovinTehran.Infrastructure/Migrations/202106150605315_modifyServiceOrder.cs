namespace NovinTehran.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyServiceOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceOrders", "GeoDivisionId", "dbo.GeoDivisions");
            DropIndex("dbo.ServiceOrders", new[] { "GeoDivisionId" });
            DropColumn("dbo.ServiceOrders", "InvoiceNumber");
            DropColumn("dbo.ServiceOrders", "GeoDivisionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceOrders", "GeoDivisionId", c => c.Int());
            AddColumn("dbo.ServiceOrders", "InvoiceNumber", c => c.String());
            CreateIndex("dbo.ServiceOrders", "GeoDivisionId");
            AddForeignKey("dbo.ServiceOrders", "GeoDivisionId", "dbo.GeoDivisions", "Id");
        }
    }
}
