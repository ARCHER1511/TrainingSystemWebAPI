using TrainingSystemAPI.Models;

namespace TrainingSystemAPI.DTO
{
    public class InstructorUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Courses { get; set; } = new List<string>();
    }
}
