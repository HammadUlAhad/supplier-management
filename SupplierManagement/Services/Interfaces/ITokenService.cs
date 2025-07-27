using SupplierManagement.Models.Api;

namespace SupplierManagement.Services.Interfaces
{
    public interface ITokenService
    {
        TokenDto GenerateToken(string username);
        bool ValidateCredentials(string username, string password);
    }
}
