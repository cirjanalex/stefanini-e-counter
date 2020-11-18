
using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using stefanini_e_counter.Logic;

namespace stefanini_e_counter.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDocumentProcessor _processor;

        public DocumentController(IWebHostEnvironment env, IDocumentProcessor processor)
        {
            _env = env;
            _processor = processor;
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

        [HttpGet]
        [Route("new")]
        [AllowAnonymous]
        public IActionResult CreateNew()
        {
            string guid = _processor.CreateDocument("Sophiexam", "Hackathon 2020");
            if ( guid == null )
                return NotFound("reference certificate not available");
            return Ok(guid);
        } 



    }
}
