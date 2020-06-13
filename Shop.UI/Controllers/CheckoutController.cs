using System;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Shop.UI.Controllers
{
    [Route("/[controller]")]
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            //var intent = // ... Fetch or create the PaymentIntent
            //ViewData["ClientSecret"] = intent.ClientSecret;
            return View();
        }
    }
}
