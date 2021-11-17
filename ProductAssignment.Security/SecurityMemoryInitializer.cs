using System;
using ProductAssignment.Core.Models;
using ProductAssignment.Core.Security;


namespace ProductAssignment.Security
{
    public class SecurityMemoryInitializer : ISecurityContextInitializer
    {
        public void Initialize(SecurityContext context)
        {
            // Delete the database, if it already exists. You need to clean and build
            // the solution for this to take effect.
            context.Database.EnsureDeleted();

            // Create the database, if it does not already exists. If the database
            // already exists, no action is taken (and no effort is made to ensure it
            // is compatible with the model for this context).
            context.Database.EnsureCreated();

            context.SaveChanges();
         
            var authenticationHelper = new AuthenticationHelper(Array.Empty<byte>()); //I send in an empty parameter, since I only need to create a password hash (Secret not needed)
            var password = "password123"; //Why not?

            //Since I'm seeding data I create a password hash + salt manually:
            authenticationHelper.CreatePasswordHash(password, out var pass, out var salt);
            context.Users.Add(new User()
            {
                Role = "Administrator",
                Username = "admin",
                PasswordHash = pass,
                PasswordSalt = salt
            });
            
            var password2 = "password456"; 

            //Since I'm seeding data I create a password hash + salt manually:
            authenticationHelper.CreatePasswordHash(password2, out var pass2, out var salt2);
            context.Users.Add(new User()
            {
                Role = "User",
                Username = "normaluser",
                PasswordHash = pass2,
                PasswordSalt = salt2
            });
            context.SaveChanges();
        }
    }
}