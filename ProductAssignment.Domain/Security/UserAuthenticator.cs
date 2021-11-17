using System.Linq;
using ProductAssignment.Core.Models;
using ProductAssignment.Domain.IRepositories;

namespace ProductAssignment.Core.Security
{
    public class UserAuthenticator: IUserAuthenticator
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationHelper _authenticationHelper;

        public UserAuthenticator(IUserRepository userRepository, IAuthenticationHelper authenticationHelper)
        {
            _userRepository = userRepository;
            _authenticationHelper = authenticationHelper;
        }

        /// <summary>
        /// Log in the user with the given credentials by returning a valid JWT token.
        /// </summary>
        /// <param name="username">The identifying username</param>
        /// <param name="password">The secret password</param>
        /// <param name="token">The generated token that is returned</param>
        /// <returns>True if the credentials given is valid</returns>
        public bool Login(string username, string password, out string token)
        {
            User user = _userRepository.GetAll().FirstOrDefault(user => user.Username.Equals(username));

            //Did we not find a user with the given username?
            if (user == null)
            {
                token = null;
                return false;
            }

            //Was the correct password not given?
            if (!_authenticationHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                token = null;
                return false;
            }

            token = _authenticationHelper.GenerateToken(user);
            return true;
        }

        /*
        public bool CreateUser(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            //Does already contain a user with the given username?
            if (user != null)
                return false;

            byte[] salt;
            byte[] passwordHash;
            _authenticationHelper.CreatePasswordHash(password, out passwordHash, out salt);

            user = new User()
            {
                Username = username,
                Role = "User",
                PasswordHash = passwordHash,
                PasswordSalt = salt
            };

            _userRepository.Add(user);

            return true;
        }
        */
    }
}