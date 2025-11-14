using System.Web.Mvc;
using backend.vitaaid.com.Code;
using backend.vitaaid.com.Model;
using backend.vitaaid.com.Models.FileManager;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace backend.vitaaid.com.Controllers
{
    public class FileManagerController : Controller
    {
        public string Name { get { return "FileManager"; } }

        public ActionResult Index()
        {
            if (!AuthHelper.IsAuthenticated())
                return RedirectToAction("SignIn", "Account");

            return View();
        }

        public ActionResult FileExplorer()
        {
            if (!AuthHelper.IsAuthenticated())
                return RedirectToAction("SignIn", "Account");

            return View();
        }
        [HttpPost]
        public ActionResult FileExplorer([Bind] FileExplorerOptions options)
        {
            if (!AuthHelper.IsAuthenticated())
                return RedirectToAction("SignIn", "Account");

            FileManagerHelper.Options = options;
            return View();
        }

        public ActionResult CustomToolbarAction(string viewType)
        {
            HttpContext.Session["aspxFileExplorerView"] = viewType == "Thumbnails" ? FileListView.Thumbnails : FileListView.Details;
            return PartialView("FileExplorerPartial", FileManagerHelper.RootFolder);
        }


        public ActionResult FileExplorerPartial()
        {
            if (!AuthHelper.IsAuthenticated())
                return RedirectToAction("SignIn", "Account");

            return PartialView("FileExplorerPartial", FileManagerHelper.RootFolder);
        }


        public FileStreamResult DownloadFiles()
        {
            return FileManagerExtension.DownloadFiles(FileManagerHelper.CreateFileManagerDownloadSettings(), (string)FileManagerHelper.RootFolder);
        }
        //public FileStreamResult DownloadImages()
        //{
        //    return FileManagerExtension.DownloadFiles(FileManagerHelper.CreateMultipleFilesSelectionDownloadSettings(), (string)FileManagerHelper.RootFolder);
        //}
    }
}