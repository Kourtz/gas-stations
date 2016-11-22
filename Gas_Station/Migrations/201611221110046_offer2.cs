namespace Gas_Station.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offer2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offers", "GasStation_Id", "dbo.GasStations");
            DropIndex("dbo.Offers", new[] { "GasStation_Id" });
            RenameColumn(table: "dbo.Offers", name: "GasStation_Id", newName: "GasStationId");
            AlterColumn("dbo.Offers", "GasStationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Offers", "GasStationId");
            AddForeignKey("dbo.Offers", "GasStationId", "dbo.GasStations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "GasStationId", "dbo.GasStations");
            DropIndex("dbo.Offers", new[] { "GasStationId" });
            AlterColumn("dbo.Offers", "GasStationId", c => c.Int());
            RenameColumn(table: "dbo.Offers", name: "GasStationId", newName: "GasStation_Id");
            CreateIndex("dbo.Offers", "GasStation_Id");
            AddForeignKey("dbo.Offers", "GasStation_Id", "dbo.GasStations", "Id");
        }
    }
}
