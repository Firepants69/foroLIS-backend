using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using foroLIS_backend.Models;

namespace foroLIS_backend.DTOs.SurveyDtos
{
    public class SurveyDto
    {
        public Guid Id { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool AllowMoreOneAnswer { get; set; }
        public IEnumerable<FieldSurveyDto>? Fields { get; set; }
    }
}
