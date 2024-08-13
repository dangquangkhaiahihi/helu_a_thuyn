namespace CSMSBE.Services.Configuration
{
    public class AuthenticationSettings
    {
        public string SecretKey { get; set; }
        public string RefreshExpireDay {  get; set; }
        public string[] AllowedAuthOrigins { get; set; }
    }
}
