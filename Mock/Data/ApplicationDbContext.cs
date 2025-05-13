using Microsoft.EntityFrameworkCore;
using Mock.Model;
using System.Reflection.Emit;

namespace Mock.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
              
        }
     

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TrainingActivityModel> TrainingActivities { get; set; }
        public DbSet<TeamSelfiesModel> TeamSelfies { get; set; }
        public DbSet<TrainingselfieModel> TrainingSelfies { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
    }
}
