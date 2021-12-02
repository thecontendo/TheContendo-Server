using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Contendo.Api.Controllers
{
    //[ApiExplorerSettings(GroupName = "api.admin")]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class InternalAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CDbContext _db;

        public InternalAuthController(IConfiguration configuration, CDbContext db)
        {
            this._configuration = configuration;
            this._db = db;
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token(LoginRequest model)
        {
            var result = new ServerResponse<JwtToken>();
            try
            {
                /*if (!ModelState.IsValid)
                {
                    result.AddError("Please enter username or password");
                    return Ok(result);
                }*/

                var user = await _db.Users
                    .Where(c => 
                        c.Username.ToLower() == model.Username.ToLower() || c.Email.ToLower() == model.Username.ToLower() 
                    ).SingleOrDefaultAsync();

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    };

                    var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["InternalAuth:Key"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["InternalAuth:Issuer"],
                        audience: _configuration["InternalAuth:Audience"],
                        expires: DateTime.Now.AddDays(2),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
                        );

                    result.Data = new JwtToken
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Email = user.Email,
                        Name = user.FirstName +" "+ user.LastName,
                        UserId = user.Id.ToString()
                    };
                    result.AddSuccess();

                    return Ok(result);
                }
                else
                {
                    result.AddError("Username does not exist");
                    return Ok(result);
                }

            }
            catch (Exception)
            {
                result.AddError("Unknown error...");
                return Ok(result);
            }
        }
        
        
    }
}