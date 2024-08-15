using Microsoft.AspNetCore.Mvc;
using PaySpace.Calculator.Web.Models;
using System.Diagnostics;

namespace PaySpace.Calculator.Web.Controllers;

public class HomeController : Controller
{
    [Route("Home/Error")]
    public IActionResult Error(int? statusCode = null)
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        if (statusCode.HasValue)
        {
            ViewBag.StatusCode = statusCode;
            switch (statusCode.Value)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you requested could not be found.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry, something went wrong on the server.";
                    break;
                default:
                    ViewBag.ErrorMessage = "An unexpected error occurred.";
                    break;
            }
        }

        return View(errorViewModel);
    }
}