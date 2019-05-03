namespace projectManagementToolWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempMigraction : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "UserRoleId");
            AddForeignKey("dbo.Users", "UserRoleId", "dbo.UserRoles", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserRoleId", "dbo.UserRoles");
            DropIndex("dbo.Users", new[] { "UserRoleId" });
        }
    }
}
