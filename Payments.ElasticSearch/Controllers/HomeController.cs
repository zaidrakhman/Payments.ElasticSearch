using Microsoft.AspNetCore.Mvc;
using Payments.ElasticSearch.Entities;
using Payments.ElasticSearch.Models;
using Payments.ElasticSearch.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.ElasticSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPaymentService _paymentService;

        public HomeController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Route("/{page:int?}")]
        public async Task<IActionResult> Index([FromRoute] int page = 0)
        {
            int paymentsPerPage = 10;
            var payments = await _paymentService.GetPayments(paymentsPerPage, paymentsPerPage * page);

            ViewData["Title"] = " - A blog about ASP.NET & Visual Studio";
            ViewData["Description"] = "A short description of the site";
            ViewData["prev"] = $"/{page + 1}/";
            ViewData["next"] = $"/{(page <= 1 ? null : page - 1 + "/")}";

            return View(payments);
        }

        [Route("/edit/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id < 0)
            {
                return View(new Payment());
            }

            var payment = await _paymentService.GetPaymentById(id);

            if (payment != null)
            {
                return View(payment);
            }

            return NotFound();
        }


        public IActionResult Privacy()
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
