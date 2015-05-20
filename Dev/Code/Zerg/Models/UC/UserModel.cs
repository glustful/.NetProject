namespace Zerg.Models.UC
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string Phone { get; set; }
        public int Status { get; set; }
    }
}