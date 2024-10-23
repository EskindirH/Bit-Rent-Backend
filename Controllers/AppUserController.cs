using BitRent.Data;
using BitRent.Models;
using BitRent.Repository;
using BitRent.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitRent.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]

    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController(IAppUser appUser) : ControllerBase
    {
        [HttpGet("index")]
        public async Task<IActionResult> List()
        {
            return Ok(await appUser.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel model)
        {
            try
            {
                var user = new AppUser()
                {
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,                    
                };

                var list = await appUser.GetAllAsync();
                if (list.Any(i => i.Email == user.Email))
                    return Conflict();
                var result = await appUser.CreateAsync(user);
                return CreatedAtAction("", new { result.Id });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await appUser.GetAsync(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditPost([FromBody] EditEmployeeViewModel model)
        {
            var result = await appUser.GetAsync(model.Id);
            if (result == null)
                return NotFound();
            try
            {
                var user = new AppUser
                {
                    Id = model.Id,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber 
                };
                var exist = await appUser.DoesExist(user);
                if (result.Email != model.Email || result.PhoneNumber != model.PhoneNumber)
                {
                    if (exist)
                        return Conflict();
                }
                    
                var usr = await appUser.UpdateAsync(user);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await appUser.GetAsync(id);
            
            try
            {
                if (result != null)
                {
                    await appUser.DeleteAsync(id);
                    return Ok();
                }
                    return NotFound();
                }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
