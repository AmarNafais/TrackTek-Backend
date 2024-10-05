using Data.Entities;
using Data.Repositories;
namespace Services;

public interface IUserService
{
    void CreateUser(User user);
    object GetUserById(long id);
    object GetAllUsers();
    void UpdateUser(User user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _UserRepository;

    public AccessUserService(IAccessUserRepository accessuserRepository, IUserContext userContext)
    {
        _accessUserRepository = accessuserRepository;
        _userContext = userContext;
    }

    public void CreateAccessUser(AccessUserCreationContract contract)
    {
        if (_userContext.UserRole == nameof(UserRoleTypeValue.SuperAdmin))
        {
            throw new AccessDeniedException();
        }
        else
        {
            Enum.TryParse(contract.IdType, out IdTypeValue idType);
            Enum.TryParse(contract.Purpose, out PurposeTypeValue purposeType);

            var newAccessUser = new AccessUser()
            {
                Title = contract.Title,
                FirstName = contract.FirstName,
                LastName = contract.LastName,
                IdNumber = contract.IdNumber,
                IdType = idType,
                Email = contract.Email,
                CountryCode = contract.CountryCode,
                PhoneNumber = contract.PhoneNumber,
                Purpose = purposeType,
                TenantId = _userContext.CurrentUserTenantId,
            };
            _accessUserRepository.Add(newAccessUser);
        }
    }

    public object GetAccessUserById(long id)
    {
        var accessUser = _accessUserRepository.GetById(id);
        return new
        {
            accessUser.Id,
            accessUser.Title,
            accessUser.FirstName,
            accessUser.LastName,
            accessUser.IdType,
            accessUser.IdNumber,
            accessUser.Email,
            accessUser.CountryCode,
            accessUser.PhoneNumber,
            accessUser.Purpose,
            accessUser.TenantId
        };
    }

    public object GetAllAccessUsers()
    {
        IEnumerable<AccessUser> accessUsers;

        if (_userContext.UserRole == nameof(UserRoleTypeValue.SuperAdmin))
        {
            accessUsers = _accessUserRepository.GetAll();
        }
        else
        {
            var tenantId = _userContext.CurrentUserTenantId;
            accessUsers = _accessUserRepository.GetAll().Where(x => x.TenantId == tenantId);
        }
        return accessUsers.Select(accessUser => new
        {
            accessUser.Id,
            accessUser.Title,
            accessUser.FirstName,
            accessUser.LastName,
            accessUser.IdNumber,
            accessUser.Email,
            accessUser.CountryCode,
            accessUser.PhoneNumber,
            accessUser.Purpose,
        });
    }

    public void UpdateAccessUser(AccessUserUpdateContract contract)
    {
        Enum.TryParse(contract.IdType, out IdTypeValue idType);
        Enum.TryParse(contract.Purpose, out PurposeTypeValue purposeType);

        var updateAccessUser = new AccessUser()
        {
            Id = contract.AccessUserId,
            Title = contract.Title,
            FirstName = contract.FirstName,
            LastName = contract.LastName,
            IdType = idType,
            IdNumber = contract.IdNumber,
            Email = contract.Email,
            CountryCode = contract.CountryCode,
            PhoneNumber = contract.PhoneNumber,
            Purpose = purposeType,
            TenantId = _userContext.CurrentUserTenantId,
        };

        _accessUserRepository.Update(updateAccessUser);
    }
}
