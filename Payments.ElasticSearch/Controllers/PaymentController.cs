using Microsoft.AspNetCore.Mvc;
using Payments.ElasticSearch.Entities;
using Payments.ElasticSearch.Services;
using System;
 using System.Threading.Tasks;

namespace Payments.ElasticSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, Payment payment)
        {
            var existing = await _paymentService.GetPaymentById(id);

            if (existing != null)
            {
                await _paymentService.SaveSingleAsync(payment);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id) 
        {
            var existing = await _paymentService.GetPaymentById(id);

            if (existing != null) 
            {
                await _paymentService.DeleteAsync(existing);
                return Ok();
            }

            return NotFound();
        }

        [HttpGet("fakeimport")]
        public async Task<ActionResult> Import()
        {
            var payments = new Payment[]
            {
                new Payment
                {
                    Id = 100001,
                    Name = "Теле2",
                    Category = "Мобильная связь",
                    Description = "Мобильная связь",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },    new Payment
                {
                    Id = 100002,
                    Name = "Beeline",
                    Category = "Мобильная связь",
                    Description = "Мобильная связь",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100003,
                    Name = "Activ",
                    Category = "Мобильная связь",
                    Description = "Мобильная связь",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100004,
                    Name = "Kcell",
                    Category = "Мобильная связь",
                    Description = "Мобильная связь",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100005,
                    Name = "Altel",
                    Category = "Мобильная связь",
                    Description = "Мобильная связь",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100007,
                    Name = "Налоги по ИИН",
                    Category = "Налоги и платежи в бюджет",
                    Description = "Налоги и платежи в бюджет",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100008,
                    Name = "Единый совокупный платеж",
                    Category = "Налоги и платежи в бюджет",
                    Description = "Налоги и платежи в бюджет",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100009,
                    Name = "Налоги по реквизитам",
                    Category = "Налоги и платежи в бюджет",
                    Description = "Налоги и платежи в бюджет",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100010,
                    Name = "Интернет Beeline",
                    Category = "Интернет",
                    Description = "Интернет",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100011,
                    Name = "Olimp",
                    Category = "Букмекерские конторы",
                    Description = "Букмекерские конторы",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100012,
                    Name = "Parimatch",
                    Category = "Букмекерские конторы",
                    Description = "Букмекерские конторы",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100013,
                    Name = "Profitbet",
                    Category = "Букмекерские конторы",
                    Description = "Букмекерские конторы",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100014,
                    Name = "Hoster.kz",
                    Category = "Хостинг",
                    Description = "Хостинг",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
                new Payment
                {
                    Id = 100015,
                    Name = "ps.kz",
                    Category = "Хостинг",
                    Description = "Попелнение счета ps.kz",
                    ReleaseDate = DateTime.Now.AddDays(-1)
                },
            };


            await _paymentService.SaveManyAsync(payments);

            return Ok();
        }
    }
}
