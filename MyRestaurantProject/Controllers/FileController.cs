using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MyRestaurantProject.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
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
    }
}