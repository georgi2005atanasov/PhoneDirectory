namespace PhoneDirectory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PhoneDirectory.Models;
    using System.Diagnostics;

    public class PhoneDirectoryController : Controller
    {
        public PhoneDirectoryController()
        {
        }

        public IActionResult All()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}