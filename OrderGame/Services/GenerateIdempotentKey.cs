using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace OrderGame.Services;

public static class GenerateIdempotentKey
{
    public static string GenerateKey(string userName, string nameOfGame)
    {
        using (var sha256 = SHA256.Create())
        {
            var input = $"{userName}{nameOfGame}";
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);
            
            Log.Information("Hash: {hash}", Convert.ToBase64String(hashBytes));
            
            return Convert.ToBase64String(hashBytes);
        }
    }
}