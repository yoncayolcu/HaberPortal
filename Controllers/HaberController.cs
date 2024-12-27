using HaberPortal.Hubs;
using HaberPortal.Models;
using HaberPortal.RepositoryInterfaces;
using HaberPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HaberPortal.Controllers
{
    public class HaberController : Controller
    {
        private readonly IHaberRepository _haberRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public HaberController(IHaberRepository haberRepository, IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _haberRepository = haberRepository;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var haberler = await _haberRepository.GetAllAsync();
            return View(haberler);
        }

        public async Task<IActionResult> Details(int id)
        {
            var haber = await _haberRepository.GetByIdAsync(id);
            if (haber == null)
            {
                return NotFound();
            }
            return View(haber);
        }
        [Authorize(Roles = ("Yazar"))]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Yazar")]
        [HttpPost]
        public async Task<IActionResult> Create(HaberCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var haber = new Haber
                {
                    Baslik = model.Baslik,
                    Icerik = model.Icerik,
                    ResimUrl = model.ResimUrl,
                    YayinTarihi = DateTime.Now,
                    YazarId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                // Haber ekleme işlemi
                await _haberRepository.AddAsync(haber);

                // SignalR ile adminlere bildirim gönder
                string yazarAdi = User.Identity.Name ?? "Bir Yazar";
                string message = $"{yazarAdi} yeni bir haber oluşturdu: {model.Baslik}";

                // SignalR Hub'ı kullanarak bildirim gönderimi
                await _hubContext.Clients.Users(await GetAdminIds()).SendAsync("ReceiveNotification", yazarAdi, message);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        private async Task<List<string>> GetAdminIds()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(admin => admin.Id).ToList();
        }


        [Authorize(Roles = ("Yazar"))]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var haber = await _haberRepository.GetByIdAsync(id);
            if (haber == null)
            {
                return NotFound();
            }

            var model = new HaberEditViewModel
            {
                Id = haber.Id,
                Baslik = haber.Baslik,
                Icerik = haber.Icerik,
                ResimUrl = haber.ResimUrl
            };

            return View(model);
        }
        [Authorize(Roles =("Yazar"))]
        [HttpPost]
        public async Task<IActionResult> Edit(HaberEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var haber = await _haberRepository.GetByIdAsync(model.Id);
            if (haber == null)
            {
                return NotFound();
            }

            haber.Baslik = model.Baslik;
            haber.Icerik = model.Icerik;
            haber.ResimUrl = model.ResimUrl;

            await _haberRepository.UpdateAsync(haber);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = ("Yazar"))]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _haberRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
