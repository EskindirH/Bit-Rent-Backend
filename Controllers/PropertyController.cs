using BitRent.Models;
using BitRent.Repository;
using BitRent.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController(IProperty property, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {

        private string ProcessUploadedFile(CreatePropertyViewModel model)
        {
            string UniqueFileName;
            string UploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "images");
            //await _blobStorageService model.Photo.OpenReadStream()
            UniqueFileName = Guid.NewGuid().ToString() + "_" +
                             Path.GetFileName(model.Photo.FileName);
            string @filePath = Path.Combine(UploadsFolder, UniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Photo.CopyTo(fileStream);
            }
            return UniqueFileName;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return Ok(await property.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreatePropertyViewModel model)
        {
            try
            {
                string uniquePath = ProcessUploadedFile(model);
                var new_prop = new Property
                {
                    Description = model.Description,
                    Price = model.Price,
                    IsAvailable = model.IsAvailable,
                    FilePath = uniquePath,
                    OwnerId = model.OwnerId,
                    CustomerId = model.CustomerId
                };
                var result = await property.CreateAsync(new_prop);
                return Ok("Property Created successfully");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await property.GetAsync(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditPost([FromBody] EditPropertyViewModel model)
        {
            var result = await property.GetAsync(model.Id);
            if (result == null)
                return NotFound();
            try
            {
                var update_prop = new Property
                {
                    Id = model.Id,
                    Description = model.Description,
                    Price = model.Price,
                    IsAvailable = model.IsAvailable,                    
                    OwnerId = model.OwnerId,
                    CustomerId = model.CustomerId
                };
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images", Path.GetFileName(model.ExistingPhotoPath));
                        System.IO.File.Delete(filePath);
                    }
                    update_prop.FilePath = ProcessUploadedFile(model);
                }

                var usr = await property.UpdateAsync(update_prop);
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
            try
            {
                var result = await property.GetAsync(id);

                if (result == null)
                {
                    await property.DeleteAsync(id);
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
