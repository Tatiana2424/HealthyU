namespace HealthuU.BLL.Model
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; } // Optional: For validating the issuer
        public string Audience { get; set; } // Optional: For validating the audience
        public int DurationInMinutes { get; set; }
    }
}
