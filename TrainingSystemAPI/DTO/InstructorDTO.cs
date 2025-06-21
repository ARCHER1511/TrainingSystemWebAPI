namespace TrainingSystemAPI.DTO
{
    public class InstructorDTO
    {
        public int Id { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public List<string>? Courses { get; set; }
    }
}
