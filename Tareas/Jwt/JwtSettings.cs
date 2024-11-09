namespace Tareas.Jwt
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }  // Clave secreta para firmar el JWT
        public string Issuer { get; set; }     // Emisor del token
        public string Audience { get; set; }   // Audiencia a la que va destinado el token
        public int ExpiryDurationInHours { get; set; } // Duración del token en horas
    }
}
