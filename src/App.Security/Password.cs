namespace App.Security;

public static class Password
{
    /// <summary>
    /// Hash a naked password
    /// </summary>
    /// <param name="nakedPassword">A naked password</param>
    /// <returns>Hashed password</returns>
    public static string Hash(string nakedPassword) => BCrypt.Net.BCrypt.HashPassword(nakedPassword, BCrypt.Net.BCrypt.GenerateSalt());

    /// <summary>
    /// Hash a naked password (prefer this)
    /// </summary>
    /// <param name="nakedPassword"></param>
    /// <returns></returns>
    public static string EnhancedHash(string nakedPassword) => BCrypt.Net.BCrypt.EnhancedHashPassword(nakedPassword);

    /// <summary>
    /// Verify naked password to the hashed one
    /// </summary>
    /// <param name="nakedPassword">A naked password</param>
    /// <param name="hashedPassword">The hashed password</param>
    /// <returns>True if verified and vice versa</returns>
    public static bool Verify(string nakedPassword, string hashedPassword) => BCrypt.Net.BCrypt.Verify(nakedPassword, hashedPassword);

    /// <summary>
    /// Verify naked password to the hashed one
    /// </summary>
    /// <param name="nakedPassword">A naked password</param>
    /// <param name="hashedPassword">The hashed password</param>
    /// <returns>True if verified and vice versa</returns>
    public static bool EnhancedVerify(string nakedPassword, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(nakedPassword, hashedPassword);
}
