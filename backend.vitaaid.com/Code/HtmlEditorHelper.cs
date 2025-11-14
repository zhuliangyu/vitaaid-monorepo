using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;
using WebDB.DBPO;

namespace backend.vitaaid.com.Code
{
    public class HtmlEditorHelper
    {
        public static int _MaxFileSize = EnvHelper.MAX_UPLOAD_SIZE;
        public static readonly UploadControlValidationSettings ImageUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = _MaxFileSize
        };

        static HtmlEditorFileSaveSettings fileSaveSettings;
        public static HtmlEditorFileSaveSettings FileSaveSettings
        {
            get
            {
                if (fileSaveSettings == null)
                {
                    fileSaveSettings = new HtmlEditorFileSaveSettings();
                    fileSaveSettings.FileSystemSettings.UploadFolder = EnvHelper.UPLOAD_DIR;
                }
                return fileSaveSettings;
            }
        }

        public static MVCxHtmlEditorImageSelectorSettings SetHtmlEditorImageSelectorSettings(MVCxHtmlEditorImageSelectorSettings settingsImageSelector, string editorName)
        {
            //settingsImageSelector.UploadCallbackRouteValues = new { Controller = "Product", Action = "HtmlEditorPartialImageSelectorUpload", EditorName = editorName };

            //settingsImageSelector.Enabled = true;
            //settingsImageSelector.CommonSettings.RootFolder = EnvHelper.BASE_DIR;
            //settingsImageSelector.CommonSettings.ThumbnailFolder = ThumbnailsDirectory;
            //settingsImageSelector.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
            //settingsImageSelector.EditingSettings.AllowCreate = true;
            //settingsImageSelector.EditingSettings.AllowDelete = true;
            //settingsImageSelector.EditingSettings.AllowMove = true;
            //settingsImageSelector.EditingSettings.AllowRename = true;
            //settingsImageSelector.UploadSettings.Enabled = true;
            //settingsImageSelector.FoldersSettings.ShowLockedFolderIcons = true;
            //settingsImageSelector.ToolbarSettings.Visible = false;
            //settingsImageSelector.FoldersSettings.Visible = false;
            //settingsImageSelector.BreadcrumbsSettings.Visible = false;

            //settingsImageSelector.PermissionSettings.AccessRules.Add(
            //    new FileManagerFolderAccessRule
            //    {
            //        Path = string.Empty,
            //        Upload = Rights.Deny
            //    });
            return settingsImageSelector;
        }


        public static void SetupGlobalUploadBehaviour(HtmlEditorSettings settings)
        {
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.CommonSettings.RootFolder = EnvHelper.BASE_DIR;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.CommonSettings.ThumbnailFolder = EnvHelper.THUMB_DIR;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.FileListSettings.DetailsViewSettings.AllowColumnDragDrop = false;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.ToolbarSettings.Visible = false;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.Enabled = true;
            settings.SettingsDialogs.InsertImageDialog.ShowFileUploadSection = true;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.FileSystemSettings.Assign(FileSaveSettings.FileSystemSettings);
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.ValidationSettings.Assign(ImageUploadValidationSettings);
            
            settings.SettingsDialogs.InsertImageDialog.SettingsImageUpload.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertImageDialog.SettingsImageSelector.UploadSettings.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertAudioDialog.SettingsAudioUpload.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertAudioDialog.SettingsAudioSelector.UploadSettings.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertFlashDialog.SettingsFlashUpload.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertFlashDialog.SettingsFlashSelector.UploadSettings.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertVideoDialog.SettingsVideoUpload.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertVideoDialog.SettingsVideoSelector.UploadSettings.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.SettingsDialogs.InsertLinkDialog.SettingsDocumentSelector.UploadSettings.ValidationSettings.MaxFileSize = _MaxFileSize;
            settings.Init = (s, e) =>
            {
                var htmlEditor = s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor;
                htmlEditor.ImageFileSaving += OnImageFileSaving;
                htmlEditor.AudioFileSaving += OnAudioFileSaving;
                htmlEditor.FlashFileSaving += OnFlashFileSaving;
                htmlEditor.VideoFileSaving += OnVideoFileSaving;

            };
        }
        public static void OnImageFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertImageDialog.SettingsImageUpload, e);
        }
        static void OnAudioFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertAudioDialog.SettingsAudioUpload, e);
        }
        static void OnFlashFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertFlashDialog.SettingsFlashUpload, e);
        }
        static void OnVideoFileSaving(object s, FileSavingEventArgs e)
        {
            PrepareFileToDelayedRemove(s as DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, he => he.SettingsDialogs.InsertVideoDialog.SettingsVideoUpload, e);
        }

        static void PrepareFileToDelayedRemove(DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor htmlEditor,
            Func<DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor, ASPxHtmlEditorUploadSettingsBase> uploadSettingsGetter,
            FileSavingEventArgs e)
        {
            e.FileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(e.FileName), Guid.NewGuid(), Path.GetExtension(e.FileName));
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath(uploadSettingsGetter(htmlEditor).UploadFolder), e.FileName);
            UploadingUtils.RemoveFileWithDelay(filePath, filePath, 1);
        }
    }
}
