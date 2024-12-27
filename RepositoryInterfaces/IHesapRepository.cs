using Microsoft.AspNetCore.Identity;

namespace HaberPortal.RepositoryInterfaces
{
    public interface IHesapRepository
    {
        Task<SignInResult> LoginAsync(string email, string password);
        Task<IdentityResult> RegisterAsync(string email, string password, string fullName);
        Task LogoutAsync();
    }
}
