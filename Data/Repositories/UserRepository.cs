using Data.Entities;

namespace Data.Repositories;

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
        var isUserExists = _repository.Users.Select(b => new { b.Email }).ToList();

        if (isUserExists.Any(b => b.Email == user.Email))
        {
            throw new Exception("Email already exists!");
        }
        else
        {
            _repository.Users.Add(user);
            _repository.SaveChanges();
            return user.Id;
        }
    }

    public User GetByUserId(long id)
    {
        return _repository.Users.FirstOrDefault(d => d.Id == id)
                ?? throw new Exception(nameof(User));
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _repository.Users;
    }

    public void UpdateUser(User user)
    {
        var userToBeUpdated = _repository.Users.FirstOrDefault(x => x.Id == user.Id)
            ?? throw new Exception(nameof(user));

        userToBeUpdated.Name = user.Name;
        userToBeUpdated.Email = user.Email;

        _repository.SaveChanges();
    }
}
