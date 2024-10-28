using Data.Entities;

namespace Data.Repositories
{
    public interface IUserRepository
    {
        long AddUser(User user);
        User GetByUserId(long id);
        IEnumerable<User> GetAllUsers();
        void UpdateUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly Repository _repository;

        public UserRepository(Repository repository)
        {
            _repository = repository;
        }

        public long AddUser(User user)
        {
            var existingUser = _repository.Users.Any(b => b.Email == user.Email);

            if (existingUser)
            {
                throw new InvalidOperationException("Email already exists!");
            }

            _repository.Users.Add(user);
            _repository.SaveChanges();
            return user.Id;
        }

        public User GetByUserId(long id)
        {
            return _repository.Users.FirstOrDefault(d => d.Id == id)
                   ?? throw new InvalidOperationException($"User with ID {id} not found.");
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repository.Users;
        }

        public void UpdateUser(User user)
        {
            var userToBeUpdated = _repository.Users.FirstOrDefault(x => x.Id == user.Id)
                                  ?? throw new InvalidOperationException($"User with ID {user.Id} not found.");

            userToBeUpdated.FirstName = user.FirstName;
            userToBeUpdated.LastName = user.LastName;
            userToBeUpdated.Email = user.Email;
            userToBeUpdated.Role = user.Role;

            _repository.SaveChanges();
        }
    }
}