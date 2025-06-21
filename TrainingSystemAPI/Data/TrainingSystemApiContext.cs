using Microsoft.EntityFrameworkCore;
using TrainingSystemAPI.Models;

namespace TrainingSystemAPI.Data
{
    public class TrainingSystemApiContext : DbContext
    {
        public TrainingSystemApiContext(DbContextOptions<TrainingSystemApiContext> options) : base(options) { }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                        .HasOne(c => c.Instructor)
                        .WithMany(i => i.Courses)
                        .HasForeignKey(i => i.InstructorId);
        }
    }
}
