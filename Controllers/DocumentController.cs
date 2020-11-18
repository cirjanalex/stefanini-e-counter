
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace stefanini_e_counter.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public DocumentController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("{guid}")]
        [AllowAnonymous]
        public IActionResult DownloadFile(string guid)
        {
            var fileInfo = _env.ContentRootFileProvider.GetFileInfo($"Resources/{guid}.docx");
            if ( !fileInfo.Exists )
                return NotFound();
            // docx mime type: https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
            var docxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; 
            return File(fileInfo.CreateReadStream(),docxMimeType, $"{guid}.docx" );
        }
    }
}
