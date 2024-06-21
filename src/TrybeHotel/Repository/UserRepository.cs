using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var userEntity = _context.Users
                .FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (userEntity == null)
                throw new Exception("Incorrect e-mail or password");

            var userDto = new UserDto
            {
                UserId = userEntity.UserId,
                Name = userEntity.Name,
                Email = userEntity.Email,
                UserType = userEntity.UserType
            };

            return userDto;
        }
        public UserDto Add(UserDtoInsert user)
        {
var isUserExists = _context.Users
    .Any(u => u.Email == user.Email);

if (isUserExists)
    throw new Exception("User email already exists");

var createdUser = new User
{
    Name = user.Name,
    Email = user.Email,
    Password = user.Password,
    UserType = "client"
};

_context.Users.Add(createdUser);
_context.SaveChanges();

var userDto = new UserDto
{
    UserId = createdUser.UserId,
    Name = createdUser.Name,
    Email = createdUser.Email,
    UserType = createdUser.UserType
};

return userDto;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
             throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    UserType = u.UserType
                });
        }

    }
}