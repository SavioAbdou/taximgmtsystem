using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using TaxiManagementSystem.DAL.Migrations;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationBaseIdentity>
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
#if DEBUG
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
#else
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
#endif
            
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<Taxi> Taxis { get; set; }
        public virtual DbSet<Lebanon> Addresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Ride>()
                .HasRequired(x => x.Source)
                .WithMany(x => x.Source)
                .HasForeignKey(x => x.SourceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ride>()
                .HasRequired(x => x.Destination)
                .WithMany(x => x.Destination)
                .HasForeignKey(x => x.DestinationId)
                .WillCascadeOnDelete(false);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUser = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
                ? System.Web.HttpContext.Current.User.Identity.Name
                : "Anonymous";

            currentUser = !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
                ? Thread.CurrentPrincipal.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).CreateUser = currentUser;
                }

                ((BaseEntity)entity.Entity).LastModificationDate = DateTime.UtcNow;
                ((BaseEntity)entity.Entity).LastModificationUser = currentUser;
            }
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }
    }
}
