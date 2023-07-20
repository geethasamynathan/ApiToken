using System.Text.Json.Serialization;

namespace JWTDemo
{
    [JsonSerializable(typeof(User))]
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public User(string username,string password)
        {
            UserName = username;
            Password = password;

        }
    }
}
