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

        public DbSet<Clients> Clients { get; set; }

        public DbSet<Projects> Projects { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<Attachments> Attachments { get; set; }

        public DbSet<ErrorLog> ErrorLog { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }

        public MGTToolContext()
            : base("name=MGTToolContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
