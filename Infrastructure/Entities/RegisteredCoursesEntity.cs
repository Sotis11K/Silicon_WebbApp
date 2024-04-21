using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class RegisteredCoursesEntity
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int CourseId { get; set; }

}
