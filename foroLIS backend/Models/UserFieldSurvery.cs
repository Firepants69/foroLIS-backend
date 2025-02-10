using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using foroLIS_backend.DTOs.SurveyDtos;

namespace foroLIS_backend.Models
{
    public class UserFieldSurvey
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }

        [Key, Column(Order = 1)]
        public Guid FieldSurveyId { get; set; }
        [ForeignKey("FieldSurveyId")]
        public virtual FieldSurvey FieldSurvey { get; set; }

        public static UserFieldSurvey DtoToModel(UserFieldSurveyDto userFieldSurveyDto)
        {
            return new UserFieldSurvey()
            {
                FieldSurveyId = userFieldSurveyDto.FieldSurveyId,

            };
        }

        public static UserFieldSurveyDto ModelToDto(UserFieldSurvey userFieldSurvey)
        {
            return new UserFieldSurveyDto()
            {
                FieldSurveyId = userFieldSurvey.FieldSurveyId,
                userId = userFieldSurvey.UserId,
            };
        }

        public static UserFieldSurvey DtoInserToModel(UserFieldInsertSurveyDto userFieldInsertSurveyDto) 
        {
            return new UserFieldSurvey()
            {
                FieldSurveyId = userFieldInsertSurveyDto.FieldSurveyId
            };
        }
    }
}
