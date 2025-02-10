using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using foroLIS_backend.DTOs.SurveyDtos;


namespace foroLIS_backend.Models
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public  bool AllowMoreOneAnswer { get; set; }
        public DateTime?  ClosingDate { get; set; }
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public ICollection<FieldSurvey> Fields { get; set; }

        public static Survey SurveyInsertToSurvey(SurveyInsertDto newSurvey)
        {
            return new Survey()
            {
                ClosingDate = newSurvey?.ClosingDate,
                PostId = newSurvey.PostId,
                AllowMoreOneAnswer = newSurvey.allowMoreOneAnswer
                
            };
        }

        public static SurveyDto SurveyToSurveyDto (Survey survey)
        {
            return new SurveyDto()
            {
                Id = survey.Id,
                ClosingDate = survey.ClosingDate,
                AllowMoreOneAnswer = survey.AllowMoreOneAnswer
                
            };
        }
        
    }
}
