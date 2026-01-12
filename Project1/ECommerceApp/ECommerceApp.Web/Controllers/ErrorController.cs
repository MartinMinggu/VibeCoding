using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                return View("NotFound");
            case 403:
                return View("Forbidden");
            case 500:
                return View("ServerError");
            default:
                return View("Error");
        }
    }
}
