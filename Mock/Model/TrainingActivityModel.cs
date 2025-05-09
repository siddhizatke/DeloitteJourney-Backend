using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mock.Model
{
    public class TrainingActivityModel
    {
        [Key]
        public int Id { get; set; }
        public required string TopicOfTheDay { get; set; }
        public required string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfTraining { get; set; }
        public required string TrainerName { get; set; }

        
    }
}
