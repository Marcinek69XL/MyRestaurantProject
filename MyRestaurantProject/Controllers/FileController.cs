using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MyRestaurantProject.Controllers
{
    [Route("file")]
    [AllowAnonymous]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[]{"fileName"})] //fileName to nazwa par. jak nizej
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var basePath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(basePath, "PrivateFiles", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.TryGetContentType(filePath, out string contentType);

            var file = System.IO.File.ReadAllBytes(filePath);
            return File(file, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest();
            }

            var basePath = Directory.GetCurrentDirectory();
            var newFileLocation = Path.Combine(basePath, "PrivateFiles", file.FileName);

            using (var stream = new FileStream(newFileLocation, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            
            return Ok();
        }
    }
}