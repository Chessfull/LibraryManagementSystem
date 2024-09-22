using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // ▼ Homepage razorview ▼
        public IActionResult Index()
        {
            return View();
        }
    }
}
