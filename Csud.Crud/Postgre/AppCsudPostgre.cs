using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models.App;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Postgre
{
    public partial class CsudPostgre : DbContext, ICsud
    {
        public DbSet<App> App { get; set; }
        public DbSet<AppDistrib> AppDistrib { get; set; }

        public DbSet<AppRoleDefinition> AppRoleDefinition { get; set; }
        public DbSet<AppRole> AppRole { get; set; }

        public DbSet<AppEntityDefinition> AppEntityDefinition { get; set; }

        public DbSet<AppEntity> AppEntity { get; set; }


        public void AppCsudPostgre(Config cfg)
        {

        }

        protected void AppOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>().HasKey(x => x.AppKey);

            modelBuilder.Entity<AppDistrib>().HasKey(x => new { x.AppKey, x.DistribKey });

            modelBuilder.Entity<AppDistrib>()
                .Property(f => f.DistribKey)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppRole>().HasKey(x => new { x.DistribKey, x.RoleName });

            modelBuilder.Entity<AppRoleDefinition>().HasKey(x => x.RoleKey);

            modelBuilder.Entity<AppEntityDefinition>().HasKey(x => x.EntityKey);

            modelBuilder.Entity<AppEntity>().HasKey(x=> new { x.DistribKey, x.EntityName});
        }
    }
}
