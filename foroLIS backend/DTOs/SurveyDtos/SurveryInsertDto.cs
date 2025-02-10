using foroLIS_backend.Models;

namespace foroLIS_backend.DTOs.SurveyDtos
{
    public class SurveyInsertDto
    {
        public DateTime? ClosingDate { get; set; }
        public  Guid PostId { get; set; }
        public bool allowMoreOneAnswer { get; set; }
        public List<FieldSurveyInsertDto> Fields {  get; set; }
    }

}
