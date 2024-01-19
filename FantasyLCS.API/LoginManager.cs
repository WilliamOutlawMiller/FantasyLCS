using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects;
using System.Security.Cryptography;
using Serilog;
using System.Text.RegularExpressions;

namespace FantasyLCS.API;
public static class LoginManager
{
    public static bool ValidateLogin(string username, string password, AppDbContext context)
    {
        var user = context.Users.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user != null)
        {
            return VerifyPasswordHash(password, user.Password);
        }
        return false;
    }

    public static string RegisterUser(SignupRequest signupData, AppDbContext context)
    {
        string usernameRegex = @"^[a-zA-Z0-9_-]+$";

        if (!Regex.IsMatch(signupData.Username, usernameRegex))
        {
            return "Username is invalid. It must only contain alphanumeric characters, underscores, or hyphens.";
        }

        if (context.Users.Any(u => u.Username.ToLower().Equals(signupData.Username.ToLower())))
        {
            return "User already exists.";
        }

        string hashedPassword = HashPassword(signupData.Password);

        var newUser = new User
        {
            Username = signupData.Username,
            Password = hashedPassword,
        };

        context.Users.Add(newUser);

        context.SaveChanges();

        return "Signup successful!";
    }

    private static bool VerifyPasswordHash(string password, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }
        return true;
    }

    // Method to hash a password
    public static string HashPassword(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }
}