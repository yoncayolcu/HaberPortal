using HaberPortal.Models;
using HaberPortal.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;

namespace HaberPortal.Repositories
{
    public class HesapRepository : IHesapRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HesapRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, false);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password, string fullName)
        {
            // Yeni kullanıcı oluştur
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            // Kullanıcı oluşturma
            var result = await _userManager.CreateAsync(user, password);

            // Eğer kullanıcı başarıyla oluşturulduysa rol ata
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Kullanıcı");
            }

            return result;
        }


        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
