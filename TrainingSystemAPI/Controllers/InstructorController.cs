using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TrainingSystemAPI.Data;
using TrainingSystemAPI.DTO;
using TrainingSystemAPI.Models;

namespace TrainingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly TrainingSystemApiContext context;
        public InstructorController(TrainingSystemApiContext _context) 
        {
            context = _context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() 
        {
            var instructors = context.Instructors.Include(i => i.Courses).ToList();

            var instructorsDTOs = instructors.Select(instructor => new InstructorDTO 
            {
                Id = instructor.Id,
                InstructorName = instructor.Name,
                Courses = instructor.Courses?.Select(course => course.Title).ToList() ?? new List<string>()
            }).ToList();

            return Ok(new GeneralResponse<List<InstructorDTO>> 
            {
                Success = true,
                Message = "Instructors retrieved successfully.",
                Data = instructorsDTOs
            });
        }
        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id) 
        {
            var instructor = context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null) 
            {
                return NotFound(new GeneralResponse<InstructorDTO>
                {
                    Success = false,
                    Message = $"Instructor with ID {id} not found.",
                    Data = "No Instructor"
                });
            }

            var instructorDTO = new InstructorDTO
            {
                Id = instructor.Id,
                InstructorName = instructor?.Name ?? "No Instructor",
                Courses = instructor?.Courses?.Select(course => course.Title).ToList() ?? new List<string>()
            };
            return Ok(new GeneralResponse<InstructorDTO> 
            {
                Success = true,
                Message = "Instructor retrieved successfully.",
                Data = instructorDTO
            });
        }
        [HttpPost("Add")]
        public IActionResult Add([FromBody] InstructorAddDTO instructorDTO) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest($"Instructor data is null or incomplete.{ModelState}");
            }
            var instructor = new Instructor 
            {
                Name = instructorDTO.Name,
                Courses = instructorDTO.Courses.Select(course => new Course { Title = course }).ToList()

            };
            context.Instructors.Add(instructor);
            context.SaveChanges();

            var createdInstructorDTO = new InstructorDTO
            {
                Id = instructor.Id,
                InstructorName = instructor.Name,
                Courses = instructor.Courses?.Select(c => c.Title).ToList() ?? new List<string>()
            };
            //return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, createdInstructorDTO);
            return Ok(new GeneralResponse<InstructorDTO> 
            {
                Success = true,
                Message = "Instructor added successfully.",
                Data = createdInstructorDTO
            });
        }
        [HttpPut("Update/{id:int}")]
        public IActionResult Update(int id,InstructorUpdateDTO instructorDTO) 
        {
            if (instructorDTO == null)
            {
                return BadRequest("Instructor data cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingInstructor = context.Instructors
                                    .Include(i => i.Courses)
                                    .FirstOrDefault(i => i.Id == id);

            if (existingInstructor == null)
            {
                return NotFound(new GeneralResponse<InstructorUpdateDTO> 
                {
                    Success = false,
                    Message = $"Instructor with ID {id} not found.",
                    Data = "no instructor"
                });
            }

            existingInstructor.Name = instructorDTO.Name;

            var matchedCourses = context.Courses
                               .Where(c => instructorDTO.Courses.Contains(c.Title))
                               .ToList();
            
            existingInstructor.Courses = matchedCourses;

            context.Instructors.Update(existingInstructor);
            context.SaveChanges();


            var instructorDto = new InstructorUpdateDTO
            {
                Name = existingInstructor.Name,
                Courses = existingInstructor.Courses.Select(c => c.Title).ToList()
            };

            return Ok(new GeneralResponse<InstructorUpdateDTO> 
            {
                Success = true,
                Message = "Instructor updated successfully.",
                Data = instructorDto
            });
        }
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id) 
        {
            var instructor = context.Instructors.FirstOrDefault(i => i.Id == id);
            if (instructor == null)
            {
                return NotFound(new GeneralResponse<string> 
                {
                    Success = false,
                    Message = $"Instructor with ID {id} not found.",
                    Data = $"{id} not deleted."
                });
            }
            context.Instructors.Remove(instructor);
            context.SaveChanges();
            return Ok(new GeneralResponse<string> 
            {
                Success = true,
                Message = $"Instructor with ID {id} deleted successfully.",
                Data = $"{id} was deleted."
            });
        }
    }
}
