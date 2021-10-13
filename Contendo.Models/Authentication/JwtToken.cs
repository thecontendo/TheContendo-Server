namespace Contendo.Models.Authentication
{
    public class JwtToken
    {
        public string Token { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string UserId { get; set; }
    }
}