using DevExpress.Web;
using System;
using System.Linq;
using WebDB.DBPO;

namespace backend.vitaaid.com.Code
{
    public class UploadControlHelper
    {
        //public const string UploadDirectory = EnvHelper.UPLOAD_DIR;
        //public const string ThumbnailFormat = "Thumbnail{0}{1}";

        public static readonly UploadControlValidationSettings UploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = EnvHelper.ALLOW_FILE_EXT,
            MaxFileSize = EnvHelper.MAX_UPLOAD_SIZE
        };

        public static void ucDragAndDrop_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                //string fileName = "S_" + Path.GetRandomFileName() + "." + FileHelper.getExtName(e.UploadedFile.FileName);
                //string resultFilePath = UploadDirectory + fileName;
                ////using (Image original = Image.FromStream(e.UploadedFile.FileContent))
                ////using (Image thumbnail = new ImageThumbnailCreator(original).CreateImageThumbnail(new Size(350, 350)))
                ////    ImageUtils.SaveToJpeg(thumbnail, HttpContext.Current.Request.MapPath(resultFilePath));
                //UploadingUtils.RemoveFileWithDelay(fileName, HttpContext.Current.Request.MapPath(resultFilePath), 5);
                //IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                //if (urlResolver != null)
                //    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
            }
        }

        public static void ucMultiSelection_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            //string resultFileName = Path.GetRandomFileName() + "_" + e.UploadedFile.FileName;
            //string resultFileUrl = UploadDirectory + resultFileName;
            //string resultFilePath = HttpContext.Current.Request.MapPath(resultFileUrl);
            //e.UploadedFile.SaveAs(resultFilePath);

            //UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

            //IUrlResolutionService urlResolver = sender as IUrlResolutionService;
            //if (urlResolver != null)
            //{
            //    string url = urlResolver.ResolveClientUrl(resultFileUrl);
            //    e.CallbackData = GetCallbackData(e.UploadedFile, url);
            //}
        }

        static string GetCallbackData(UploadedFile uploadedFile, string fileUrl)
        {
            string name = uploadedFile.FileName;
            long sizeInKilobytes = uploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";

            return name + "|" + fileUrl + "|" + sizeText;
        }

    }
}
