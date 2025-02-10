
namespace foroLIS_backend.DTOs.SurveyDtos
{
    public class FieldSurveyDto
    {
        public int NumberOfSelections { get; set; }
        public bool IsSelected { get; set; }
        public string Title { get; set; }
        public Guid SuveryId { get; set; }
        public Guid Id { get; set; }
    }
}
