namespace Mock.Model
{
    public class TrainingselfieModel
    {
        public int Id { get; set; }
        public required string TrainingImageBase64 { get; set; }
        public required string TrainingDescription { get; set; }
    }
    public class TrainingSelfieUploadDto
    {
        public int Id { get; set; }
        public required string TrainingDescription { get; set; }
        public required IFormFile TrainingImage { get; set; }
    }
}
