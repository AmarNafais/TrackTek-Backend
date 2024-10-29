namespace Data.Entities
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public DateTime ExpirationTime { get; set; }

        public PasswordResetToken(string email, string resetCode)
        {
            Email = email;
            ResetCode = resetCode;
            ExpirationTime = DateTime.UtcNow.AddMinutes(5);
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationTime;
        }
    }
}
