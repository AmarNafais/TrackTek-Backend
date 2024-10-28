using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.DTOs;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

public interface IUserService
{
    void CreateUser(CreateUserDTO userDto);
    object GetUserById(int id);
    object GetAllUsers();
    void UpdateUser(UpdateUserDTO userDto);
    User GetUserByEmailAndPassword(string email, string password);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IEmailService emailService, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
    }

    public void CreateUser(CreateUserDTO userDto)
    {
        var generatedPassword = GenerateRandomPassword(8);
        Enum.TryParse(userDto.Role, out UserRole userRole);
        var newUser = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Role = userRole,
            Password = _passwordHasher.HashPassword(new User(), generatedPassword)
        };

        _emailService.SendWelcomeEmail(newUser, generatedPassword);
        _userRepository.AddUser(newUser);
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
            user.Role
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
            user.Role
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
            _userRepository.UpdateUser(updateUser);
        }
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

    private string GenerateRandomPassword(int length)
    {
        const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var password = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            var byteArray = new byte[1];
            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(byteArray);
                var randomIndex = byteArray[0] % validCharacters.Length;
                password.Append(validCharacters[randomIndex]);
            }
        }
        return password.ToString();
    }
}
