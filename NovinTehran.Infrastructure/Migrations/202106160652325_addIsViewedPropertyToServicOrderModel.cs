namespace NovinTehran.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsViewedPropertyToServicOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceOrders", "IsViewed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceOrders", "IsViewed");
        }
    }
}
