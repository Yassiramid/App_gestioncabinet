namespace Infrastructure.DataAccess.Entities
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string position { get; set; }
        public string email { get; set; }
        public byte[] profile { get; set; }
    }
}
