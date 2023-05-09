namespace projectManagementToolWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 150),
                        Extention = c.String(maxLength: 25),
                        FileType = c.String(maxLength: 50),
                        Document = c.Binary(storeType: "image"),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .Index(t => t.ProjectID);
           
           
            
        }
        
        public override void Down()
        {
           
            DropForeignKey("dbo.Attachments", "ProjectID", "dbo.Projects");           
            DropIndex("dbo.Attachments", new[] { "ProjectID" });           
            DropTable("dbo.Attachments");
        }
    }
}
