using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public interface IAuthService
{
    void CreateUser(CreateUserDTO userDto);
    string AuthenticateUser(string email, string password);
    void RequestPasswordReset(string email);
    bool VerifyResetCode(string email, string code);
    void ResetPassword(string email, string newPassword, string confirmPassword, string code);
}

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly string _jwtKey;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

    public AuthService(
        IUserRepository userRepository,
        IUserService userService,
        IConfiguration config,
        IEmailService emailService,
        IPasswordHasher<User> passwordHasher,
        IPasswordResetTokenRepository passwordResetTokenRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtKey = config["Jwt:Key"];
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _passwordResetTokenRepository = passwordResetTokenRepository;
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
            Password = _passwordHasher.HashPassword(new User(), generatedPassword),
            IsActive = true
        };

        _emailService.SendWelcomeEmail(newUser, generatedPassword);
        _userRepository.AddUser(newUser);
    }

    public string AuthenticateUser(string email, string password)
    {
        var user = _userService.GetUserByEmailAndPassword(email, password);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public void RequestPasswordReset(string email)
    {
        var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == email);
        if (user == null)
            throw new ArgumentException("User with specified email does not exist.");

        var resetCode = GenerateResetCode();
        var resetToken = new PasswordResetToken(email, resetCode)
        {
            Email = email,
            ResetCode = resetCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5)
        };
        _passwordResetTokenRepository.AddToken(resetToken);
        _emailService.SendResetCodeEmail(user, resetCode);
    }
    public bool VerifyResetCode(string email, string code)
    {
        var token = _passwordResetTokenRepository.GetTokenByEmail(email);
        if (token == null || token.IsExpired() || token.ResetCode != code)
        {
            return false;
        }

        return true;
    }
    public void ResetPassword(string email, string newPassword, string confirmPassword, string code)
    {
        if (newPassword != confirmPassword)
            throw new ArgumentException("Passwords do not match.");

        if (!VerifyResetCode(email, code))
            throw new UnauthorizedAccessException("Invalid or expired reset code.");

        var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == email);
        if (user == null)
            throw new ArgumentException("User with specified email does not exist.");

        user.Password = _passwordHasher.HashPassword(user, newPassword);
        _userRepository.UpdateUser(user);
        var token = _passwordResetTokenRepository.GetTokenByEmail(email);
        if (token != null)
        {
            _passwordResetTokenRepository.DeleteToken(token);
        }
    }

    private string GenerateRandomPassword(int length)
    {
        const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var password = new StringBuilder();

        using var rng = RandomNumberGenerator.Create();
        var byteArray = new byte[1];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(byteArray);
            var randomIndex = byteArray[0] % validCharacters.Length;
            password.Append(validCharacters[randomIndex]);
        }

        return password.ToString();
    }

    private string GenerateResetCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}