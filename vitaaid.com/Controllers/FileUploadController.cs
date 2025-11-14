using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace vitaaid.com.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/FileUpload")]
  public class FileUploadController : ControllerBase
  {
    private IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IWebHostEnvironment environment, ILogger<FileUploadController> logger)
    {
      _logger = logger;
      _hostEnvironment = environment;
    }

    //[HttpPost]
    //public IActionResult Post(string category = "ProductImages", string options = "normal") {
    //    IFormFileCollection files = Request.Form.Files;
    //    string uniqueFileName = "";

    //    if (files.Count > 0) {
    //        var file = files[0];
    //        uniqueFileName = string.Format("{0}{1}{2}",
    //            (category) switch
    //            {
    //                "ProductImages" => "ProductImages/product/",
    //                _ => "ProductImages/product/"
    //            },
    //            (options) switch
    //            {
    //                "large" => "large/",
    //                _ => ""
    //            },
    //            file.FileName);//Path.GetExtension(file.FileName));

    //        try {
    //            var path = Path.Combine(_hostEnvironment.WebRootPath, uniqueFileName);

    //            using(Stream fileStream = new FileStream(path, FileMode.Create)) {
    //                file.CopyTo(fileStream);
    //            }
    //        } catch {
    //            return BadRequest("Failed to save a file on the server");
    //        }
    //    }

    //    return Ok(uniqueFileName);
    //}
  }
}
