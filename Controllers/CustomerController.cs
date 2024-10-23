using BitRent.Repository;
using BitRent.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomer customer) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await customer.GetAll());
        }

        
    }
}
