namespace foroLIS_backend.DTOs
{
    public class JwtSettings
    {
        public string? Key {get; set;}
        public string ValidISsuer { get; set;}
        public string ValidAudience { get; set;}
        public decimal Expires { get; set;}

    }
}
