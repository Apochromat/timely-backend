namespace timely_backend.Models.DTO
{
    public class DuplicateResponse
    {
        public string? Message { get; set; }
        public string? Status { get; set; }
        public IList<LessonDTO>? InvalidLessons { get; set; }
    }
}
