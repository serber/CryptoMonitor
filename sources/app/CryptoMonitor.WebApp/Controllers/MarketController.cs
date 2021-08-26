using CryptoMonitor.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptoMonitor.WebApp.Models.Market;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CryptoMonitor.WebApp.Controllers
{
    [Authorize]
    [Route("market")]
    [Route("")]
    public class MarketController : Controller
    {
        private readonly IMediator _mediator;

        public MarketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("data")]
        public async Task<IActionResult> Data([FromQuery] FilterMarketModel model)
        {
            var (symbolPrices, totalCount) = await _mediator.Send(model.ToQuery());

            return Ok(new {recordsTotal = totalCount, recordsFiltered = totalCount, data = symbolPrices });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}