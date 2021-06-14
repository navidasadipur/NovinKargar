namespace NovinTehran.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAnotationsOfServiceOrders : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceOrders", "CustomerFirstName", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.ServiceOrders", "CustomerLastName", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.ServiceOrders", "Address", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.ServiceOrders", "Phone", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceOrders", "PostalCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceOrders", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceOrders", "Email", c => c.String());
            AlterColumn("dbo.ServiceOrders", "PostalCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.ServiceOrders", "Phone", c => c.String(maxLength: 50));
            AlterColumn("dbo.ServiceOrders", "Address", c => c.String(maxLength: 500));
            AlterColumn("dbo.ServiceOrders", "CustomerLastName", c => c.String(maxLength: 500));
            AlterColumn("dbo.ServiceOrders", "CustomerFirstName", c => c.String(maxLength: 500));
        }
    }
}
