using Microsoft.AspNetCore.Mvc;

namespace VendaLanches.Controllers;

public class ContatoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
