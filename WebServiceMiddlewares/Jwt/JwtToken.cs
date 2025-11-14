namespace vitaaid.com.Jwt
{
  public class JwtToken
    {
        //Token
        public string access_token { get; set; }
        //Refresh Token
        public string refresh_token { get; set; }
        // expired in seconds
        public int expires_in { get; set; }
    }
}
