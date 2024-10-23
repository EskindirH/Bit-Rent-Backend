using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BitRent.Data;
using BitRent.Models;
using BitRent.Repository;
using BitRent.ViewModel;


namespace BitRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAppUser appUser;
        private readonly ICustomer customer;
        private readonly IOwner owner;
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;

        public AccountController(IAppUser appUser, ICustomer customer, IOwner owner, IConfiguration configuration,
                                 AppDbContext context)
        {
            this.appUser = appUser;
            this.customer = customer;
            this.owner = owner;
            this.configuration = configuration;
            this.context = context;
        }

        private string? GenerateToken(AppUser? usr = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBitRent:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (usr != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usr.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, usr.FirstName),
                    new Claim("lastname", usr.LastName),
                    new Claim(ClaimTypes.Email, usr.Email),
                    new Claim(ClaimTypes.MobilePhone, usr.PhoneNumber)
                };
                var token = new JwtSecurityToken(configuration["JwtBitRent:Issuer"],
                                               configuration["JwtBitRent:Audience"],
                                               claims,
                                               expires: DateTime.Now.AddHours(2),
                                               signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }          
            return null;
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passworSalt)
        {
            using var hmac = new HMACSHA512(passworSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private async Task<AppUser> AuthenticateUser(UserLogin user)
        {
            var currentUser = await context.AppUsers.FirstOrDefaultAsync(
                p => p.Email == user.Email);

            if (currentUser != null)
            {
                if (VerifyPasswordHash(user.Password!, currentUser.PasswordHash, currentUser.PasswordSalt))
                    return currentUser;
            }
            return null!;
        }

        private static void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            UserLogin model = new()
            {
                ReturnUrl = returnUrl
            };
            return Ok(model);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserLogin model, string? returnUrl)
        {
            var user = await AuthenticateUser(model);
            if (user != null)
            {
                if (GenerateToken(user) != null)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Ok(returnUrl);
                    }
                    //UserId = user.Id;
                    HttpContext.Response.Headers.Append("Bearer", GenerateToken(user));

                    return Ok(user);
                }
            }
            return Unauthorized(model);
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterUser model)
        {
            try
            {
                HashPassword(model.Password!, out byte[] PasswordHash, out byte[] PasswordSalt);
              
                if (model.Discriminator == "Customer")
                {
                    Customer cust = new()
                    {
                        FirstName = model.Firstname,
                        LastName = model.Lastname,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        AccountNumber = model.AccountNumber!,
                        PasswordHash = PasswordHash,
                        PasswordSalt = PasswordSalt
                    };
                    var exist = await customer.DoesExist(cust);
                    if (exist)
                    {
                        ModelState.AddModelError("", "User Already Exist!");
                        return Conflict();
                    }
                    var result = await customer.CreateAsync(cust);
                    return Ok();
                }
                else if (model.Discriminator == "Owner")
                {
                    Owner user = new()
                    {
                        FirstName = model.Firstname,
                        LastName = model.Lastname,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        AccountNumber = model.AccountNumber!,
                        PasswordHash = PasswordHash,
                        PasswordSalt = PasswordSalt
                    };
                    var exist = await owner.DoesExist(user);
                    if (exist)
                    {
                        ModelState.AddModelError("", "User Already Exist!");
                        return Conflict();
                    }
                    var result = await owner.CreateAsync(user);
                    return Ok();
                }
                else
                {
                    AppUser user = new()
                    {
                        FirstName = model.Firstname,
                        LastName = model.Lastname!,
                        Email = model.Email!,
                        PhoneNumber = model.PhoneNumber,
                        PasswordHash = PasswordHash,
                        PasswordSalt = PasswordSalt
                    };
                    var userExist = await appUser.DoesExist(user);
                    if (userExist)
                    {
                        ModelState.AddModelError("", "User Already Exist!");
                        return Conflict();
                    }
                    var result = await appUser.CreateAsync(user);
                    return Ok();
                }
                
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
