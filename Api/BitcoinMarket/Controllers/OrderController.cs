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
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BitcoinMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class OrderController : ControllerBase
    {
        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddOrder([FromBody] JObject data)
        {
            var errorMessage = ""; var limitValue = 0.0m;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? errorMessage : "Not logged in";

            errorMessage = int.TryParse(data["type"]?.ToString(), out var type) ? errorMessage : "Order type not valid";
            errorMessage = decimal.TryParse(data["orderValue"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var orderValue) && orderValue > 0 ? errorMessage : "Order value not valid";
            errorMessage = bool.TryParse(data["isBuy"]?.ToString(), out var isBuy) ? errorMessage : "IsBuy argument not valid";
            if ((OrderType)type == (OrderType.LimitOrder))
                errorMessage = decimal.TryParse(data["limitValue"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out limitValue) && limitValue > 0 ? errorMessage : "Limit value not valid";

            var addOrderErrorMessage = await _orderRepository.AddOrder(userId, isBuy, type, orderValue, limitValue);
            errorMessage = addOrderErrorMessage.Length <= 0 ? errorMessage : addOrderErrorMessage;

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var errorMessage = "";;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? errorMessage : "Not logged in";


            var deleteOrderErrorMessage = await _orderRepository.RemoveOrder(userId, orderId);
            errorMessage = deleteOrderErrorMessage.Length <= 0 ? errorMessage : deleteOrderErrorMessage;

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            return Ok();
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestOrders(int page, int pageSize)
        {
            var latestOrders = await _orderRepository.GetLatestOrders(page, pageSize);
            return Ok(latestOrders);
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetLatestSells(int page, int pageSize)
        {
            var latestOffers = await _orderRepository.GetLatestSells(page, pageSize);
            return Ok(latestOffers);
        }


        [HttpGet("buys")]
        public async Task<IActionResult> GetLatestBuys(int page, int pageSize)
        {
            var latestOffers = await _orderRepository.GetLatestBuys(page, pageSize);
            return Ok(latestOffers);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var gottenOrder = await _orderRepository.GetOrderById(id);

            if (gottenOrder != null)
                return Ok(gottenOrder);

            return NotFound();
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAllOrders()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";
            errorMessage = _userRepository.IsUserAdmin(userId) ? errorMessage : "Not admin";
            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var users = await _orderRepository.GetAllOrders();

            if (users != null)
                return Ok(users);

            return Forbid();
        }

        [Authorize]
        [HttpGet("active-orders")]
        public async Task<IActionResult> GetActiveUserOrders(int page, int pageSize)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var orders = await _orderRepository.GetActiveOrdersByUserId(userId, page, pageSize);
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("finished-orders")]
        public async Task<IActionResult> GetFinishedOrdersByUserId(int page, int pageSize)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var orders = await _orderRepository.GetFinishedOrdersByUserId(userId, page, pageSize);
            return Ok(orders);
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChartData(int userId)
        {
            var chartData = await _orderRepository.AggregateChartData();
            return Ok(chartData);
        }

        [Authorize]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            return Ok(_orderRepository.GetStats());
        }
    }
}
