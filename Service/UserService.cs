using Data.Entities;
using Data.Repositories;
namespace Services;

public interface IUserService
{
    void CreateUser(User user);
    object GetUserById(int id);
    object GetAllUsers();
    void UpdateUser(User user);
    User GetUserByEmailAndPassword(string email, string password);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void CreateUser(User user)
    {
        var newUser = new User()
        {
            Name = user.Name,
            Email = user.Email,
        };
        _userRepository.AddUser(newUser);
    }

    public object GetUserById(int id)
    {
        var user = _userRepository.GetByUserId(id);
        return new
        {
            user.Id,
            user.Name,
            user.Email,
        };
    }

    public object GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        return users.Select(user => new
        {
            user.Id,
            user.Name,
            user.Email,
        });
    }

    public void UpdateUser(User user)
    {
        var updateUser = new User()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };

        _userRepository.UpdateUser(updateUser);
    }
    public User GetUserByEmailAndPassword(string email, string password)
    {
        var user = _userRepository.GetAllUsers()
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
            return null;

        if (user.Password == password)
            return user;

        return null;
    }
}
