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
    => optionsBuilder.UseSqlServer(
        "" +
        "Integrated Security=False;" +
        "Trusted_Connection=False;" +
        "TrustServerCertificate=True", o =>  o.EnableRetryOnFailure());

        //Integrated Security=False;
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


            //modelBuilder.Entity<Category>(e => e.HasData(
            //    new Category { Id = 1, Name = "مطالعه" },new Category { Id = 2, Name = "اوقات فراغت"},
            //    new Category { Id = 3, Name = "ورزش"},new Category { Id = 4, Name = "کار"},
            //    new Category { Id = 5, Name = "آرامش"},new Category { Id = 7, Name = "نظافت"},
            //    new Category { Id = 8, Name = "خرید"},new Category { Id = 9, Name = "سرگرمی"},
            //    new Category { Id = 10, Name = "خانواده"}
            //    ));
        }
    }
}
