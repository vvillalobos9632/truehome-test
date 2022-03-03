namespace Common.DTOs.Settings
{
    public class TokenValidationParametersData
    {
        public bool ValidateIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public bool ValidateLifetime { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public string SecurityKey { get; set; }

        public int MinExpiresToken { get; set; }
    }
}
