using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class SavedCourseEntity
{
    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }
}
