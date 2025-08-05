namespace Infrastructure.Common.Cache
{
    public static class ActiveUser
    {
        public static int c_id { get; set; }
        public static string c_userName { get; set; }
        public static string c_password { get; set; }
        public static string c_firstName { get; set; }
        public static string c_lastName { get; set; }
        public static string c_position { get; set; }
        public static string c_email { get; set; }
        public static byte[] c_profile { get; set; }
    }
}
