namespace _470_Final_AGAIN.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3242 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TrainingDatas");
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        TrainingSet_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TrainingDatas", t => t.TrainingSet_ID)
                .Index(t => t.TrainingSet_ID);
            
            AlterColumn("dbo.TrainingDatas", "ID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.TrainingDatas", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "TrainingSet_ID", "dbo.TrainingDatas");
            DropIndex("dbo.Users", new[] { "TrainingSet_ID" });
            DropPrimaryKey("dbo.TrainingDatas");
            AlterColumn("dbo.TrainingDatas", "ID", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.Users");
            AddPrimaryKey("dbo.TrainingDatas", "ID");
        }
    }
}
