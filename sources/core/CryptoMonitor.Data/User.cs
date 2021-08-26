namespace CryptoMonitor.Data
{
    public class User
    {
        public string UserId { get; set; }

        public string Login { get; set; }
        
        public string PasswordHash { get; set; }
    }
}