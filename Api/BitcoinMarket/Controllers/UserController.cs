﻿using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using BitcoinMarket.Data.Models;
using BitcoinMarket.Repositories.Interfaces;
using BitcoinMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BitcoinMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]

    public class UserController : ControllerBase
    {
        public UserController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;


        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var gottenUser = await _userRepository.GetUserById(id);

            if (gottenUser != null)
                return Ok(gottenUser);

            return NotFound();
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAllUsers()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";
            errorMessage = _userRepository.IsUserAdmin(userId) ? errorMessage : "Not admin";
            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            var users = await _userRepository.GetAllUsers(userId);

            if (users != null)
                return Ok(users);

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
        public async Task<IActionResult> AddUser([FromBody]JObject userData)
        {
            var username = userData["username"]?.ToString();
            var password = userData["password"]?.ToString();
            if (username?.Length < 6 || password?.Length < 6)
                return BadRequest("Invalid username/password");

            if(await _userRepository.GetUserByUsername(username) != null)
                return BadRequest("User with this username already exists");

            var hashedPassword = _authService.HashPassword(password);
            var wasAddSuccessful = await _userRepository.Register(username, hashedPassword);
            if (wasAddSuccessful)
                return Ok();
            else
                return BadRequest("Something went wrong during user creation");
        }

        [Authorize]
        [HttpPost("admin-remove-user")]
        public async Task<IActionResult> AdminRemoveUser([FromBody] JObject data)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";
            errorMessage = int.TryParse(data["userToRemoveId"]?.ToString(), out var userToRemoveId) ? errorMessage : "User id to remove not valid";
            errorMessage = _userRepository.IsUserAdmin(userId) ? errorMessage : "Not admin";
            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            await _userRepository.RemoveUser(userToRemoveId);
            return Ok();
        }


        [Authorize]
        [HttpGet("admin")]
        public async Task<IActionResult> IsUserAdmin()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) && _userRepository.IsUserAdmin(userId));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]JObject userData)
        {
            var username = userData["username"]?.ToString();
            var password = userData["password"]?.ToString();
            if (username == null || password == null)
                return BadRequest();

            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
                return BadRequest("Username not found");

            var passwordValid = _authService.VerifyPassword(password, user.Password);
            if (!passwordValid)
                return BadRequest("Invalid password");

            var authData = _authService.GetAuthData(user.Id);
            var loginResponse = new LoginResponseData
            {
                Id = authData.Id,
                Token = authData.Token,
                TokenExpirationTime = authData.TokenExpirationTime,
                UsdBalance = user.UsdBalance,
                BtcBalance = user.BtcBalance
            };

            return Ok(loginResponse);
        }

        [Authorize]
        [HttpPost("balance")]
        public async Task<IActionResult> SetUserBalance([FromBody] JObject data)
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var errorMessage = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out var userId) ? "" : "Not logged in";

            errorMessage = decimal.TryParse(data["balanceUsd"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var usdBalance) && usdBalance > 0 ? errorMessage : "USD balance value not valid";
            errorMessage = decimal.TryParse(data["balanceBtc"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var btcBalance) && btcBalance > 0 ? errorMessage : "BTC balance value not valid";

            if (errorMessage.Length > 0)
                return BadRequest(errorMessage);

            await _userRepository.SetUserBalance(userId, usdBalance, btcBalance);
            return Ok();
        }
    }
}
