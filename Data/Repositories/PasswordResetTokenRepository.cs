using Data.Entities;

namespace Data.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        void AddToken(PasswordResetToken token);
        PasswordResetToken GetTokenByEmail(string email);
        void DeleteToken(PasswordResetToken token);
        void DeleteExpiredTokens();
    }

    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly Repository _repository;

        public PasswordResetTokenRepository(Repository repository)
        {
            _repository = repository;
        }

        public void AddToken(PasswordResetToken token)
        {
            var existingToken = _repository.PasswordResetTokens
                .Where(t => t.Email == token.Email && t.ExpirationTime > DateTime.UtcNow)
                .FirstOrDefault();

            if (existingToken != null)
            {
                throw new InvalidOperationException("A valid token already exists for this email.");
            }

            _repository.PasswordResetTokens.Add(token);
            _repository.SaveChanges();
        }

        public PasswordResetToken GetTokenByEmail(string email)
        {
            return _repository.PasswordResetTokens
                .FirstOrDefault(t => t.Email == email && t.ExpirationTime > DateTime.UtcNow)
                ?? throw new InvalidOperationException("No valid token found for the provided email.");
        }


        public void DeleteToken(PasswordResetToken token)
        {
            _repository.PasswordResetTokens.Remove(token);
            _repository.SaveChanges();
        }

        public void DeleteExpiredTokens()
        {
            var expiredTokens = _repository.PasswordResetTokens
                                           .Where(t => t.IsExpired())
                                           .ToList();

            if (expiredTokens.Any())
            {
                _repository.PasswordResetTokens.RemoveRange(expiredTokens);
                _repository.SaveChanges();
            }
        }
    }
}