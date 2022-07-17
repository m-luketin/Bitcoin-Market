using BitcoinMarket.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Services
{
    public interface IAuthService
    {
        bool VerifyPassword(string password1, string password2);
        AuthData GetAuthData(int id);
        string HashPassword(string password);
    }
}
