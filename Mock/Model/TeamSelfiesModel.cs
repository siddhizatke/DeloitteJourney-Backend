namespace Mock.Model
{
    public class TeamSelfiesModel
    {
        public int Id { get; set; }
        public required string TeamImageBase64 { get; set; }
        public required string TeamselfieDescription { get; set; }
        
    }
    public class TeamSelfieUploadDto
    {
        public int Id { get; set; }
        public required string TeamDescription { get; set; }
        public required IFormFile TeamImage { get; set; }
    }

}
