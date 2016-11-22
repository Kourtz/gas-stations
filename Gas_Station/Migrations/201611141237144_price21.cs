namespace Gas_Station.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class price21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GasStations", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.GasStations", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GasStations", "Longitude", c => c.Single(nullable: false));
            AlterColumn("dbo.GasStations", "Latitude", c => c.Single(nullable: false));
        }
    }
}
