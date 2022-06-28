using BitcoinMarket.Data;
using BitcoinMarket.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("add")]
        public async Task<IActionResult> AddTrade(Trade tradeToAdd)
        {
            var wasAddSuccessful = await _tradeRepository.AddTrade(tradeToAdd);
            if (wasAddSuccessful)
                return Ok();

            return StatusCode(422);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestTrades(int page, int pageSize)
        {
            var latestTrades = await _tradeRepository.GetLatestTrades(page, pageSize);
            return Ok(latestTrades);
        }

        [HttpGet("offers")]
        public async Task<IActionResult> GetLatestOffers(int page, int pageSize)
        {
            var latestOffers = await _tradeRepository.GetLatestOffers(page, pageSize);
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

        [HttpGet("user-trades")]
        public async Task<IActionResult> GetAllTradesByUserId(int userId)
        {
            var trades = await _tradeRepository.GetAllTradesByUserId(userId);
            return Ok(trades);
        }

        [HttpGet("user-sales")]
        public async Task<IActionResult> GetAllSalesByUserId(int userId)
        {
            var trades = await _tradeRepository.GetSellTradesByUserId(userId);
            return Ok(trades);
        }

        [HttpGet("user-purchases")]
        public async Task<IActionResult> GetAllPurchasesByUserId(int userId)
        {
            var trades = await _tradeRepository.GetBuyTradesByUserId(userId);
            return Ok(trades);
        }
    }
}
