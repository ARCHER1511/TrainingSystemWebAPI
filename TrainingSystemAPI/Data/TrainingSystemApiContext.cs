using Microsoft.EntityFrameworkCore;
using TrainingSystemAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TrainingSystemAPI.Data
{
    public class TrainingSystemApiContext : IdentityDbContext<AppUser>
    {
        public TrainingSystemApiContext(DbContextOptions<TrainingSystemApiContext> options) : base(options) { }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>()
                        .HasOne(c => c.Instructor)
                        .WithMany(i => i.Courses)
                        .HasForeignKey(i => i.InstructorId);
        }
    }
}
