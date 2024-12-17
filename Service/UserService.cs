using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.DTOs;

public interface IUserService
{
    object GetUserById(int id);
    object GetAllUsers();
    void UpdateUser(UpdateUserDTO userDto);
    void UpdateUserStatus(int userId);
    User GetUserByEmailAndPassword(string email, string password);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public object GetUserById(int id)
    {
        var user = _userRepository.GetByUserId(id);
        return new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            Role = user.Role.ToString(),
            user.IsActive
        };
    }

    public object GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        return users.Select(user => new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            Role = user.Role.ToString(),
            user.IsActive
        });
    }

    public void UpdateUser(UpdateUserDTO userDto)
    {
        var updateUser = _userRepository.GetByUserId(userDto.Id);
        Enum.TryParse(userDto.Role, out UserRole userRole);
        if (updateUser != null)
        {
            updateUser.FirstName = userDto.FirstName;
            updateUser.LastName = userDto.LastName;
            updateUser.Email = userDto.Email;
            updateUser.Role = userRole;
            updateUser.IsActive = userDto.IsActive;
            _userRepository.UpdateUser(updateUser);
        }
    }

    public void UpdateUserStatus(int userId)
    {
        var userToUpdate = _userRepository.GetByUserId(userId);
        if (userToUpdate == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (userToUpdate.IsActive == true)
        {
            userToUpdate.IsActive = false;
        }
        else
        {
            userToUpdate.IsActive = true;
        }
        _userRepository.UpdateUser(userToUpdate);
    }

    public User GetUserByEmailAndPassword(string email, string password)
    {
        var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == email);
        if (user == null)
            return null;
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (passwordVerificationResult == PasswordVerificationResult.Success)
            return user;
        return null;
    }
}
