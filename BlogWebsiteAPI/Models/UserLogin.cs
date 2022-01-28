namespace BlogWebsiteAPI.Models
{
    public class UserLogin
    {
        //Class Unnecessary at current moment come back to later
        public UserLogin(string userName, string password)
        {

        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Salt { get; set; }
        public int UserId { get; set; }
    }
}
