using System.Threading.Tasks;
using CryptoMonitor.Services.Commands;
using CryptoMonitor.WebApp.Models.DropPrice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitor.WebApp.Controllers
{
    [Authorize]
    [Route("dropprice")]
    [ApiController]
    public class DropPriceController : ProtectedController
    {
        private readonly IMediator _mediator;

        public DropPriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> AddAsync(AddDropPriceModel model)
        {
            var userId = UserId;

            await _mediator.Send(new AddDropPriceCommand
            {
                UserId = userId,
                SellSymbol = model.SellSymbol,
                BuySymbol = model.BuySymbol,
                SymbolSource = model.SymbolSource,
                Price = model.Price
            });

            return Ok();
        }

        [HttpGet("data")]
        public async Task<IActionResult> DataAsync([FromQuery] FilterDropPriceModel model)
        {
            var userId = UserId;

            var (dropPrices, totalCount) = await _mediator.Send(model.ToQuery(userId));

            return Ok(new { recordsTotal = totalCount, recordsFiltered = totalCount, data = dropPrices });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(DeleteDropPriceModel model)
        {
            return Ok();
        }
    }
}