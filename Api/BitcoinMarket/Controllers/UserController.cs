using BitcoinMarket.Data;
using BitcoinMarket.Repositories.Interfaces;
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
    public class UserController : ControllerBase
    {
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        private readonly IUserRepository _userRepository;


        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var gottenUser = await _userRepository.GetUserById(id);

            if (gottenUser != null)
                return Ok(gottenUser);

            return NotFound();
        }

        [HttpGet("username")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var gottenUser = await _userRepository.GetUserByUsername(username);

            if (gottenUser != null)
                return Ok(gottenUser);

            return NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(User userToAdd)
        {
            var wasAddSuccessful = await _userRepository.Register(userToAdd);
            if (wasAddSuccessful)
                return Ok();

            return StatusCode(422);
        }

        [HttpGet("trade")]
        public async Task<IActionResult> GetAllUsersByTradeId(int userId)
        {
            var users = await _userRepository.GetUsersByTradeId(userId);
            return Ok(users);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var users = await _userRepository.Login(username, password);
            return Ok(users);
        }
    }
}
