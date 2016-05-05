namespace _470_Final_AGAIN.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainingDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Salty = c.String(),
                        Sour = c.String(),
                        Sweet = c.String(),
                        Bitter = c.String(),
                        Meaty = c.String(),
                        Piquant = c.String(),
                        Rating = c.String(),
                        PrepTime = c.String(),
                        ShowUser = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TrainingDatas");
        }
    }
}
