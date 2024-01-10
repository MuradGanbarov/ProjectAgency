using Microsoft.AspNetCore.Mvc;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.Controllers
{
    [Area("ProjectAgencyAdmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
