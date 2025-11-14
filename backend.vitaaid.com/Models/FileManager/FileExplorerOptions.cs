using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace backend.vitaaid.com.Models.FileManager
{
    public class FileExplorerOptions
    {
        FileManagerSettingsEditing settingsEditing;
        FileManagerSettingsToolbar settingsToolbar;
        FileManagerSettingsFolders settingsFolders;
        FileManagerSettingsFileList settingsFileList;
        FileManagerFileListDetailsViewSettings settingsDetailsView;
        FileManagerSettingsBreadcrumbs settingsBreadcrumbs;
        MVCxFileManagerSettingsUpload settingsUpload;

        public FileExplorerOptions()
        {
            this.settingsEditing = new FileManagerSettingsEditing(null)
            {
                AllowCreate = true,
                AllowMove = true,
                AllowDelete = true,
                AllowRename = true,
                AllowCopy = true,
                AllowDownload = true
            };
            this.settingsToolbar = new FileManagerSettingsToolbar(null)
            {
                ShowPath = true,
                ShowFilterBox = true
            };
            this.settingsFolders = new FileManagerSettingsFolders(null)
            {
                Visible = true,
                EnableCallBacks = false,
                ShowFolderIcons = true,
                ShowLockedFolderIcons = true
            };
            this.settingsFileList = new FileManagerSettingsFileList(null)
            {
                ShowFolders = true,
                ShowParentFolder = false
            };
            this.settingsBreadcrumbs = new FileManagerSettingsBreadcrumbs(null)
            {
                Visible = false,
                ShowParentFolderButton = true,
                Position = BreadcrumbsPosition.Top
            };
            this.settingsDetailsView = new FileManagerFileListDetailsViewSettings(null)
            {
                AllowColumnResize = true,
                AllowColumnDragDrop = true,
                AllowColumnSort = true,
                ShowHeaderFilterButton = true,
            };
            this.settingsUpload = new MVCxFileManagerSettingsUpload();
            this.settingsUpload.Enabled = true;
            this.settingsUpload.AdvancedModeSettings.EnableMultiSelect = true;
        }

        [Display(Name = "Settings Editing")]
        public FileManagerSettingsEditing SettingsEditing { get { return settingsEditing; } }
        [Display(Name = "Settings Toolbar")]
        public FileManagerSettingsToolbar SettingsToolbar { get { return settingsToolbar; } }
        [Display(Name = "Settings Folders")]
        public FileManagerSettingsFolders SettingsFolders { get { return settingsFolders; } }
        [Display(Name = "Settings FileList")]
        public FileManagerSettingsFileList SettingsFileList { get { return settingsFileList; } }
        [Display(Name = "Settings Breadcrumbs")]
        public FileManagerSettingsBreadcrumbs SettingsBreadcrumbs { get { return settingsBreadcrumbs; } }
        [Display(Name = "Settings DetailsView")]
        public FileManagerFileListDetailsViewSettings SettingsDetailsView { get { return settingsDetailsView; } }
        [Display(Name = "Settings Upload")]
        public MVCxFileManagerSettingsUpload SettingsUpload { get { return settingsUpload; } }
    }
}
