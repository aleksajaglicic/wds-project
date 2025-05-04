namespace Trainer.Server.Helpers
{
    using Trainer.Server.Interfaces;

    public class PasswordHasher : IPasswordHasher
    {
        #region Methods
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        #endregion
    }
}
