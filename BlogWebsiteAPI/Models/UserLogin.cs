namespace BlogWebsiteAPI.Models
{
    public class UserLogin
    {
        public UserLogin(string userName, string password, int )
        {

        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Salt { get; set; }
        public int UserId { get; set; }
    }
}
