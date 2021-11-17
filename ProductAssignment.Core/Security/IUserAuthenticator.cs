namespace ProductAssignment.Core.Security
{
    public interface IUserAuthenticator
    {
        /// <summary>
        /// Log in the user with the given credentials by returning a valid JWT token.
        /// </summary>
        /// <param name="username">The identifying username</param>
        /// <param name="password">The secret password</param>
        /// <param name="token">The generated token that is returned</param>
        /// <returns>True if the credentials given is valid</returns>
        bool Login(string username, string password, out string token);

        //bool CreateUser(string username, string password);
    }
}