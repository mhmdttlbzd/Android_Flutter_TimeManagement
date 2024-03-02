using BackEnd_WebApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace BackEnd_WebApi.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("sqlcmd -S timemanagementdb,1433 -Usa -PUY77ZGoNXSdcGp5n6miN9yVi");


        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ApplicationTask> Tasks { get; set; }
        public DbSet<TimeHistory> TimeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationTask>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Tasks).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.HasNoKey());
            modelBuilder.Entity< IdentityUserRole< string>>(entity => entity.HasNoKey());
            modelBuilder.Entity< IdentityUserToken<string>>(entity => entity.HasNoKey());
        }
    }
}
