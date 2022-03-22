using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageFiles;
using TrzyszczCMS.Core.Infrastructure.Shared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManageFiles
    {
        #region Fields
        private bool fileUploadVisible = false;
        private bool addFilesAllowed = false;
        private bool deleteFilesAllowed = false;
        #endregion

        #region Properties
        [CascadingParameter]
        private Popupper Popupper { get; set; }

        public string CssClassOfFileUploadVisibility =>
            CssClassesHelper.ClassCollapsingElement(fileUploadVisible && addFilesAllowed);
        #endregion

        #region Init
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );

            this.addFilesAllowed    = await AuthService.HasClearanceAsync(PolicyClearance.AllowFilesAdding);
            this.deleteFilesAllowed = await AuthService.HasClearanceAsync(PolicyClearance.AllowFilesDeleting);

            this.ViewModel.OnFilesUploadFailure = new ((s, failedFileUploads) =>
            {
                this.Popupper.HideProgress();
                string message = failedFileUploads == 1 ?
                    $"1 file upload has finished with errors." :
                    $"{failedFileUploads} file uploads have finished with errors.";
                this.Popupper.ShowAlert(message);
            });
            this.ViewModel.OnFilesUploadBegin = new((s, e) =>
            {
                string fileNoun = this.ViewModel.FilesForUpload.Count == 1 ? "file" : "files";
                this.Popupper.ShowProgress(
                    $"Uploading {this.ViewModel.FilesForUpload.Count} {fileNoun}...",
                    this.ViewModel.FilesForUpload.Count
                );
            });
            this.ViewModel.OnFilesUploadSuccess = new((s, e) =>
                this.Popupper.HideProgress()
            );
            this.ViewModel.OnFilesUploadSingleFileSuccess = new((s, countOfUploadedFiles) =>
                this.Popupper.ProgressCurrentValue = countOfUploadedFiles
            );
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await this.ViewModel.LoadFilesAsync();
            }
        }
        #endregion

        #region Methods
        private void ToggleFileUploadVisibility() =>
            fileUploadVisible = !fileUploadVisible;

        private void CreateDirectory()
        {
            Popupper.ShowPrompt("Enter the name for a new directory.", async answer =>
            {
                if (answer.GetValue(out string name, out _))
                {
                    if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                    {
                        Popupper.ShowAlert("A name of the directory cannot be empty.");
                    }
                    else if (!await ViewModel.CreateDirectoryForCurrentNodeAsync(name))
                    {
                        Popupper.ShowAlert("The directory with the specified name has been existed over there or the name is forbidden for use.");
                    }
                }
            }, true, maxLength: Constraints.ContFile.NAME);
        }
        
        private void OnSelectedFiles(InputFileChangeEventArgs e)
        {
            this.ViewModel.FilesForUpload = null;
            
            var sizeExceedingFiles = e.GetMultipleFiles().Where(i => i.Size > CommonConstants.MAX_UPLOADED_FILE_LENGTH_BYTES);
            if (sizeExceedingFiles.Any())
            {
                var names = HtmlHelper.MakeUnorderedList(sizeExceedingFiles.Select(i =>
                    $"{i.Name} ({Math.Ceiling(i.Size / 1048576.0):N0} MB)"
                ));

                names.Insert(0, $"The following files exceeded the maximum allowed file size ({CommonConstants.MAX_UPLOADED_FILE_LENGTH_MEGABYTES} MB):<br/>");
                Popupper.ShowAlert(names.ToString());
                return;
            }

            this.ViewModel.FilesForUpload = e.GetMultipleFiles();
        }

        private void DeleteFile(SimpleFileInfo file)
        {
            this.Popupper.ShowYesNoPrompt($"Delete <em>{file.Name}</em>?", async result =>
            {
                if (result == PopupExitResult.Yes)
                {
                    await ViewModel.DeleteFileAsync(file.Id);
                }
            });
        }
        #endregion
    }
}
