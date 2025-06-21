namespace TrainingSystemAPI.DTO
{
    public class InstructorAddDTO
    {
        public string Name { get; set; }
        public List<string> Courses { get; set; } = new List<string>();
    }
}
