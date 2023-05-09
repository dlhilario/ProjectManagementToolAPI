namespace projectManagementToolWebAPI.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MGTToolContext : DbContext
    {
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<MaterialList> MaterialList { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }


        public MGTToolContext()
            : base("name=MGTToolContext")
        {
            Database.SetInitializer<MGTToolContext>(new CreateDatabaseIfNotExists<MGTToolContext>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
