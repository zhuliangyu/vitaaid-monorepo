namespace vitaaid.com.Jwt
{
  public class JwtPayload
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        // expired datetime
        public int exp { get; set; }
    }
}
