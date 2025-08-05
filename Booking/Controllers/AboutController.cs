using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Booking.Controllers;

public class AboutController : Controller
{
        public IActionResult About()
        {
            // Exemple de données à envoyer à la vue
            string message = "Bienvenue dans ma première vue ASP.NET !";
            return View(model: message);
        }
    

}