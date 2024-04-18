/*namespace WebbApp.ViewModels
{
    public class CourseViewModel
    {

        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Course>? Courses { get; set; }


        public int Id { get; set; }
        public bool IsBestSeller { get; set; }
        public string Image { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? DiscountPrice { get; set; }
        public string Hours { get; set; } = null!;
        public string LikesInProcent { get; set; } = null!;
        public string LikesInNumbers { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
*/







namespace WebbApp.ViewModels
{
    public class CourseViewModel
    {
        public IEnumerable<Category>? Categories { get; set; }
        public Pagination? Pagination { get; set; }
        public IEnumerable<Course>? Courses { get; set; }

    }
}