namespace agendaBackEnd.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }


    }
}
