using Csud.Crud.Models.App;
using Csud.Crud.Models.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Postgre
{
    public partial class CsudPostgre : DbContext, ICsud
    {
        public DbSet<App> App { get; set; }
        public DbSet<AppDistrib> AppDistrib { get; set; }
        public DbSet<AppRoleDefinition> AppRoleDefinition { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<AppRoleDetails> AppRoleDetails { get; set; }
        public DbSet<AppEntityDefinition> AppEntityDefinition { get; set; }
        public DbSet<AppEntity> AppEntity { get; set; }
        public DbSet<AppAttributeDefinition> AppAttributeDefinition { get; set; }
        public DbSet<AppAttribute> AppAttribute { get; set; }
        public DbSet<AppOperationDefinition> AppOperationDefinition { get; set; }
        public DbSet<AppOperation> AppOperation { get; set; }
        public DbSet<AppImport> AppImport { get; set; }

        public void AppCsudPostgre(Config cfg)
        {

        }

        protected void AppOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>().HasKey(x => x.AppKey);

            modelBuilder.Entity<AppDistrib>().HasKey(x => new {x.AppKey, x.DistribKey});

            modelBuilder.Entity<AppDistrib>()
                .Property(f => f.DistribKey)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppRole>().HasKey(x => new {x.DistribKey, x.RoleName});

            modelBuilder.Entity<AppRoleDefinition>().HasKey(x => x.RoleKey);

            modelBuilder.Entity<AppRoleDetails>().HasKey(x => new {x.RoleKey, x.OperationKey});

            modelBuilder.Entity<AppEntityDefinition>().HasKey(x => x.EntityKey);

            modelBuilder.Entity<AppEntity>().HasKey(x => new {x.DistribKey, x.EntityName});

            modelBuilder.Entity<AppAttributeDefinition>().HasKey(x => x.AttributeKey);

            modelBuilder.Entity<AppAttribute>().HasKey(x => new {x.DistribKey, x.AttributeType});

            modelBuilder.Entity<AppOperationDefinition>().HasKey(x => x.OperationName);

            modelBuilder.Entity<AppOperation>().HasKey(x => new {x.DistribKey, x.OperationName});

            modelBuilder.Entity<AppImport>().HasKey(x => x.Key);
        }
    };
}
