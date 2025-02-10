
namespace foroLIS_backend.DTOs.SurveyDtos
{
    public class UserFieldSurveyDto
    {
        public Guid FieldSurveyId { get; set; }
        public string? userId { get; set; }
    }

    public class UserFieldInsertSurveyDto
    {
        public Guid FieldSurveyId { get; set; }
    }

}
