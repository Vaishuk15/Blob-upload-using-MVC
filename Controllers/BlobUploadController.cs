using BlobUpload.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlobUpload.Controllers
{
    public class BlobUploadController : Controller
    {
        private readonly IBlobUploadService _blobUploadService;
        private readonly ILogger _logger;

        public BlobUploadController(ILogger<BlobUploadController> logger, IBlobUploadService blobUploadService)
        {
            _logger = logger;
            _blobUploadService = blobUploadService;
        }
        //public ActionResult Upload()
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile photo)
        {
            var imageUrl = await _blobUploadService.UploadInBlob(photo);
            TempData["LatestImage"] = imageUrl.ToString();
            return RedirectToAction("LatestImage");
            //return View(imageUrl);
        }

        public ActionResult LatestImage()
        {
            var latestImage = string.Empty;
            if (TempData["LatestImage"] != null)
            {
                ViewBag.LatestImage = Convert.ToString(TempData["LatestImage"]);
            }

            return View();
        }
    }
}
