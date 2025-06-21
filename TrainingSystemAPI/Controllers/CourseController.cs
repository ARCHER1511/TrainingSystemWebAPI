using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystemAPI.Data;
using TrainingSystemAPI.DTO;
using TrainingSystemAPI.Models;

namespace TrainingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController: ControllerBase
    {
        private readonly TrainingSystemApiContext context;
        public CourseController(TrainingSystemApiContext _context)
        {
            context = _context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var courses = context.Courses.Include(c=> c.Instructor).ToList();

            var coursesDTOs = courses.Select(course => new CourseDTO
            {
                Id = course.Id,
                CourseName = course.Title,
                InstructorName = course.Instructor?.Name ?? "No Instructor"
            }).ToList();
            return Ok(new GeneralResponse<List<CourseDTO>> 
            {
                Success = true,
                Message = "Courses retrieved successfully.",
                Data = coursesDTOs
            });
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var course = context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefault(c => c.Id == id);

            var courseDTO = new CourseDTO 
            {
                Id = course?.Id ?? 0,
                CourseName = course?.Title ?? "No Course",
                InstructorName = course?.Instructor?.Name ?? "No Instructor"
            }; 
            return Ok(new GeneralResponse<CourseDTO> 
            {
                Success = true,
                Message = course != null ? "Course retrieved successfully." : $"Course with ID {id} not found.",
                Data = courseDTO
            });
        }
        [HttpPost("Add")]
        public IActionResult Add([FromBody] CourseAddDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Course data is null or incomplete.{ModelState}");
            }
            var instructor = context.Instructors.FirstOrDefault(i => i.Id == courseDTO.InstructorId);
            if (instructor == null)
            {
                return NotFound(new GeneralResponse<CourseDTO> 
                {
                    Success = false,
                    Message = $"Instructor with the course {courseDTO.Title} not found.",
                    Data = "No instructor for the course"
                });
            }
            var course = new Course
            {
                Title = courseDTO.Title,
                InstructorId = courseDTO.InstructorId,
                Instructor = instructor
            };
            context.Courses.Add(course);
            context.SaveChanges();
            var createdCourseDTO = new CourseDTO
            {
                Id = course.Id,
                CourseName = course.Title,
                InstructorName = instructor.Name
            };
            //return CreatedAtAction(nameof(GetById), new { id = course.Id }, createdCourseDTO);
            return Ok(new GeneralResponse<CourseDTO> 
            {
                Success = true,
                Message = "Course added successfully.",
                Data = createdCourseDTO
            });
        }
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] CourseUpdateDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Course data is null or incomplete.{ModelState}");
            }
            var course = context.Courses.Include(c => c.Instructor).FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound(new GeneralResponse<CourseDTO> 
                {
                    Success = false,
                    Message = $"Course with ID {id} not found.",
                    Data = "No Course"
                });
            }
            var instructor = context.Instructors.FirstOrDefault(i => i.Id == courseDTO.InstructorId);
            if (instructor == null)
            {
                return NotFound(new GeneralResponse<CourseDTO>{
                    Success = false,
                    Message = $"Instructor with ID {courseDTO.InstructorId} not found.",
                    Data = "No Instructor"
                });
            }
            course.Title = courseDTO.Title;
            course.InstructorId = courseDTO.InstructorId;
            course.Instructor = instructor;
            context.SaveChanges();
            var updatedCourseDTO = new CourseDTO
            {
                Id = course.Id,
                CourseName = course.Title,
                InstructorName = instructor.Name
            };
            return Ok(new GeneralResponse<CourseDTO> 
            {
                Success = true,
                Message = "Course updated successfully.",
                Data = updatedCourseDTO
            });
        }
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound(new GeneralResponse<string> 
                {
                    Success = false,
                    Message = $"Course with ID {id} not found.",
                    Data = $"{id} not deleted"
                });
            }
            context.Courses.Remove(course);
            context.SaveChanges();
            return Ok(new GeneralResponse<string>
            {
                Success = true,
                Message = $"Course with ID {id} deleted successfully.",
                Data = $"{id} was deleted"
            });
        }
    }
}
