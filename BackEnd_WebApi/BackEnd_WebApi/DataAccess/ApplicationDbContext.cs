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
    => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TimeManagementDb;Trusted_Connection=True;TrustServerCertificate=True");


        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Tag> Tags { get; set; }    
        public DbSet<ApplicationTask> Tasks { get; set; }
        public DbSet<TimeHistory> TimeHistories { get; set; }
    }
}
