using TrainingSystemAPI.Models;

namespace TrainingSystemAPI.DTO
{
    public class CourseAddDTO
    {
        public string Title { get; set; } = string.Empty;
        public int InstructorId { get; set; }
    }
}
