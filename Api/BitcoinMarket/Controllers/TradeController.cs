using BitcoinMarket.Data;
using BitcoinMarket.Data.Enums;
using BitcoinMarket.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BitcoinMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class TradeController : ControllerBase
    {
        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }
        private readonly ITradeRepository _tradeRepository;

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTrade([FromBody] JObject data)
        {
            var errorMessage = ""; var limitValue = 0.0m;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? errorMessage : "Not logged in";

            errorMessage = int.TryParse(data["type"]?.ToString(), out var type) ? errorMessage : "Trade type not valid";
            errorMessage = decimal.TryParse(data["tradeValue"]?.ToString(), out var tradeValue) && tradeValue > 0 ? errorMessage : "Trade value not valid";
            errorMessage = bool.TryParse(data["isBuy"]?.ToString(), out var isBuy) ? errorMessage : "IsBuy argument not valid";
            if ((TradeType)type == (TradeType.LimitOrder))
                errorMessage = decimal.TryParse(data["limitValue"]?.ToString(), out limitValue) && limitValue > 0 ? errorMessage : "Limit value not valid";

            var addTradeErrorMessage = await _tradeRepository.AddTrade(userId, isBuy, type, tradeValue, limitValue);
            errorMessage = addTradeErrorMessage.Length <= 0 ? errorMessage : addTradeErrorMessage;

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteTrade(int tradeId)
        {
            var errorMessage = "";;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? errorMessage : "Not logged in";


            var deleteTradeErrorMessage = await _tradeRepository.RemoveTrade(userId, tradeId);
            errorMessage = deleteTradeErrorMessage.Length <= 0 ? errorMessage : deleteTradeErrorMessage;

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            return Ok();
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestTrades(int page, int pageSize)
        {
            var latestTrades = await _tradeRepository.GetLatestTrades(page, pageSize);
            return Ok(latestTrades);
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetLatestSells(int page, int pageSize)
        {
            var latestOffers = await _tradeRepository.GetLatestSells(page, pageSize);
            return Ok(latestOffers);
        }


        [HttpGet("buys")]
        public async Task<IActionResult> GetLatestBuys(int page, int pageSize)
        {
            var latestOffers = await _tradeRepository.GetLatestBuys(page, pageSize);
            return Ok(latestOffers);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTrade(int id)
        {
            var gottenTrade = await _tradeRepository.GetTradeById(id);

            if (gottenTrade != null)
                return Ok(gottenTrade);

            return NotFound();
        }

        [Authorize]
        [HttpGet("finished-trades")]
        public async Task<IActionResult> GetActiveUserTrades(int page, int pageSize)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var trades = await _tradeRepository.GetActiveTradesByUserId(userId, page, pageSize);
            return Ok(trades);
        }

        [Authorize]
        [HttpGet("active-trades")]
        public async Task<IActionResult> GetFinishedTradesByUserId(int page, int pageSize)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var trades = await _tradeRepository.GetFinishedTradesByUserId(userId, page, pageSize);
            return Ok(trades);
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChartData(int userId)
        {
            var chartData = await _tradeRepository.AggregateChartData();
            return Ok(chartData);
        }
    }
}
