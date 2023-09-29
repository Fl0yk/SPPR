using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using System.IO;
using WEB_153501_Kosach.IdentityServer.Models;

namespace WEB_153501_Kosach.IdentityServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AvatarController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _defaultAvatar;

        public AvatarController(UserManager<ApplicationUser> userManager,
                            IWebHostEnvironment environment,
                            IConfiguration configuration) 
        {
            _environment = environment;
            _userManager = userManager;
            _defaultAvatar = configuration.GetSection("DefaultAvatarName").Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            if (id is null)
                return BadRequest("User not found");

            string searcPattern = $@"{id}.*";
            var files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "Images"), searcPattern);

            string imagePath;
            if (files.Any())
            {
                imagePath = files[0];
            }
            else
            {
                imagePath = Path.Combine(_environment.WebRootPath, "Images", _defaultAvatar);
            }

            FileStream fs = new(imagePath, FileMode.Open);
            string ext = Path.GetExtension(imagePath);

            var extProvider = new FileExtensionContentTypeProvider();
            return File(fs, extProvider.Mappings[ext]);

        }
    }
}
