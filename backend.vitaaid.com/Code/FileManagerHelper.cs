using backend.vitaaid.com.Models.FileManager;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebDB.DBPO;

namespace backend.vitaaid.com.Code
{
    public class FileManagerHelper
    {
        static HttpContext Context { get { return HttpContext.Current; } }

        public static readonly object FileManagerFolder = EnvHelper.BASE_DIR;
        public static readonly object RootFolder = EnvHelper.BASE_DIR;
        public static readonly string[] AllowedFileExtensions = EnvHelper.ALLOW_FILE_EXT;
        public static FileExplorerOptions Options
        {
            get
            {
                if (Context.Session["FileExplorerOptions"] == null)
                    Context.Session["FileExplorerOptions"] = new FileExplorerOptions();
                return (FileExplorerOptions)Context.Session["FileExplorerOptions"];
            }
            set { Context.Session["FileExplorerOptions"] = value; }
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerDownloadSettings()
        {
            return CreateFileManagerDownloadSettingsCore(Options.SettingsEditing);
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateMultipleFilesSelectionDownloadSettings()
        {
            var editingSettings = new DevExpress.Web.FileManagerSettingsEditing(null)
            {
                AllowDownload = true
            };
            return CreateFileManagerDownloadSettingsCore(editingSettings);
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerGeneralDownloadSettings()
        {
            FileManagerSettingsEditing editingSettings = CreateFileManagerEditingSettings();
            return CreateFileManagerDownloadSettingsCore(editingSettings);
        }
        static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerDownloadSettingsCore(FileManagerSettingsEditing editingSettings)
        {
            var settings = new DevExpress.Web.Mvc.FileManagerSettings();
            settings.SettingsEditing.Assign(editingSettings);
            settings.Name = "fileManager";
            return settings;
        }
        public static FileManagerSettingsEditing CreateFileManagerEditingSettings()
        {
            return new FileManagerSettingsEditing(null)
            {
                AllowCreate = true,
                AllowMove = true,
                AllowDelete = true,
                AllowRename = true,
                AllowCopy = true,
                AllowDownload = true
            };
        }

        public static List<SelectListItem> GetFileListViewModes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = FileListView.Thumbnails.ToString(), Value = FileListView.Thumbnails.ToString(), Selected = true },
                new SelectListItem() { Text = FileListView.Details.ToString(), Value = FileListView.Details.ToString() }
            };
        }
        public static List<SelectListItem> GetFileListPageSizes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "50", Value = "50" },
                new SelectListItem() { Text = "100", Value = "100" },
                new SelectListItem() { Text = "300", Value = "300", Selected = true },
                new SelectListItem() { Text = "500", Value = "500" }
            };
        }
    }
}
