using Core.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using TrzyszczCMS.Client.Data.Enums;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// This class is used for making popups visible in Layouts.
    /// </summary>
    public class Popupper
    {
        #region Fields
        /// <summary>
        /// The pressed prompt result.
        /// </summary>
        private PopupExitResult _exitResult;
        /// <summary>
        /// Invoked with typed prompt result.
        /// </summary>
        private Action<Result<string, object>> _promptResult;
        #endregion

        #region Events
        /// <summary>
        /// Notification about model update for view component.
        /// </summary>
        public event EventHandler NotifyModelUpdated;
        #endregion

        #region Properties
        /// <summary>
        /// The typed text in the input.
        /// </summary>
        public string TypedInput { get; set; }
        /// <summary>
        /// Displayed message.
        /// </summary>
        public MarkupString Message { get; private set; }
        /// <summary>
        /// Type of displayed popup.
        /// </summary>
        public PopupType PopupType { get; private set; }
        #endregion

        #region Ctor
        public Popupper()
        {
            this.Message    = new MarkupString(string.Empty);
            this.TypedInput = string.Empty;
            this.PopupType  = PopupType.None;
        }
        #endregion

        #region Methods :: Show methods
        /// <summary>
        /// Dipslay a card with a message and a prompt.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <param name="promptResultAction">THe action executed after pressing OK button</param>
        /// <param name="cancellable">Is cancellation option displayed</param>
        /// <returns>Typed string</returns>
        public void ShowPrompt(string message, Action<Result<string, object>> promptResultAction, bool cancellable)
        {
            this._promptResult = promptResultAction;
            this.SetDisplayData(message, cancellable ? PopupType.CancellablePrompt : PopupType.Prompt);
        }
        /// <summary>
        /// Dipslay a card with a message only.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <returns>Typed string</returns>
        public void ShowAlert(string message)
        {
            this._promptResult = null;
            this.SetDisplayData(message, PopupType.Alert);
        }
        #endregion

        #region Methods :: Button press exiting methods
        public void OnOK()
        {
            var returnedText = this.TypedInput;
            this.SetReturnData(PopupExitResult.OK);
            this._promptResult?.Invoke(Result<string, object>.MakeSuccess(returnedText));
        }
        public void OnCancel()
        {
            this.SetReturnData(PopupExitResult.Cancel);
            this._promptResult?.Invoke(Result<string, object>.MakeError());
        }
        #endregion

        #region Helper methods
        private void SetDisplayData(string message, PopupType popupType)
        {
            this.Message   = new MarkupString(message);
            this.PopupType = popupType;
            this.NotifyModelUpdated.Invoke(this, EventArgs.Empty);
        }
        private void SetReturnData(PopupExitResult exitResult)
        {
            this._exitResult = exitResult;
            this.PopupType   = PopupType.None;
            this.TypedInput  = string.Empty;
            this.NotifyModelUpdated.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
