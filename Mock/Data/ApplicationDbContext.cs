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

            modelBuilder.Entity<UserModel>().HasData(
              new UserModel
                {
                  Id = 1,
                  Name = "Siddhi Zatke",
                 AboutMe = "A passionate developer.",
                 AboutMeFormal= "A passionate developer with a keen interest in software development and technology.",
                  ProfilePictureUrl = "\"C:\\Users\\szatke\\Pictures\\New Photo.jpg\"",
                // PhotosUrl = new List<string> { "\"C:\\Users\\szatke\\Pictures\\New Photo.jpg\"", "\"C:\\Users\\szatke\\Pictures\\photo.jpg\"" }
                 });
        }
     

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TrainingActivityModel> TrainingActivities { get; set; }
        public DbSet<TeamSelfiesModel> TeamSelfies { get; set; }
        public DbSet<TrainingselfieModel> TrainingSelfies { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
    }
}
