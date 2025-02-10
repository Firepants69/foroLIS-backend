using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using foroLIS_backend.DTOs.SurveyDtos;

namespace foroLIS_backend.Models
{
    public class FieldSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }

        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        public static FieldSurvey DtoInsertToFieldSurvey(Guid surveyId, FieldSurveyInsertDto fieldSurveyInsertDto)
        {
            return new FieldSurvey()
            {
                Title = fieldSurveyInsertDto.Title,
                SurveyId = surveyId
            };
        }
        public static FieldSurveyDto FieldSuveryToDto(FieldSurvey fieldSurvey)
        {
            return new FieldSurveyDto()
            {
                Id = fieldSurvey.Id,
                SuveryId = fieldSurvey.SurveyId,
                Title = fieldSurvey.Title
            };
        }
    }
}
