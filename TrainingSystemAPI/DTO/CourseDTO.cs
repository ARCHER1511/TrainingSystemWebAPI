namespace TrainingSystemAPI.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int InstructorId { get; set; }
    }
}
