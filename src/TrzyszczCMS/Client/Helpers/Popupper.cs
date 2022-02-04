using Core.Shared.Helpers;
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
        private Action<Result<string, object>> _promptStringResult;
        /// <summary>
        /// Invoked with a pressed closing button.
        /// </summary>
        private Action<PopupExitResult> _promptEnumResult;
        /// <summary>
        /// The stored value for <see cref="ProgressCurrentValue"/> property.
        /// </summary>
        private int _progressCurrentValue;
        /// <summary>
        /// The stored value for <see cref="ProgressMaxValue"/> property.
        /// </summary>
        private int _progressMaxValue;
        #endregion

        #region Events
        /// <summary>
        /// Notification about model update for view component.
        /// </summary>
        public event EventHandler NotifyModelUpdated;
        #endregion

        #region Properties
        /// <summary>
        /// Type of displayed popup.
        /// </summary>
        public PopupType PopupType { get; private set; }
        /// <summary>
        /// The typed text in the input.
        /// </summary>
        public string TypedInput { get; set; }
        /// <summary>
        /// Displayed message.
        /// </summary>
        public MarkupString Message { get; private set; }
        /// <summary>
        /// The current value of the progress.
        /// </summary>
        public int ProgressCurrentValue
        {
            get => this._progressCurrentValue;
            set
            {
                if (this.PopupType != PopupType.Progress)
                {
                    throw ExceptionMaker.Member.Invalid(PopupType, nameof(PopupType));
                }
                this._progressCurrentValue = value;
                this.NotifyModelUpdated.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// The max reachable value of the progress.
        /// </summary>
        public int ProgressMaxValue
        {
            get => this._progressMaxValue;
            set
            {
                if (this.PopupType != PopupType.Progress)
                {
                    throw ExceptionMaker.Member.Invalid(PopupType, nameof(PopupType));
                }
                this._progressMaxValue = value;
                this.NotifyModelUpdated.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Max length of the input text.
        /// </summary>
        public int MaxInputPromptLength { get; set; }
        #endregion

        #region Ctor
        public Popupper()
        {
            this._progressMaxValue     = 1;
            this._progressCurrentValue = 0;
            this.PopupType             = PopupType.None;
            this.Message               = new MarkupString(string.Empty);
            this.TypedInput            = string.Empty;
            this.MaxInputPromptLength  = 255;
        }
        #endregion

        #region Methods :: Show methods
        /// <summary>
        /// Display a card with a message and a prompt.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <param name="promptResultAction">The action executed after pressing OK button</param>
        /// <param name="cancellable">Is cancellation option displayed</param>
        /// <returns>Typed string</returns>
        public void ShowPrompt(string message, Action<Result<string, object>> promptResultAction, bool cancellable, int maxLength = 255)
        {
            this._promptStringResult = promptResultAction;
            this.MaxInputPromptLength = maxLength;
            this.SetDisplayData(message, cancellable ? PopupType.CancellablePrompt : PopupType.Prompt);
        }
        /// <summary>
        /// Dipslay a card with a message only.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <returns>Typed string</returns>
        public void ShowAlert(string message)
        {
            this._promptStringResult = null;
            this.SetDisplayData(message, PopupType.Alert);
        }
        /// <summary>
        /// Display a card with a message and result buttons.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <param name="promptResultAction">The action executed after pressing a result button</param>
        public void ShowYesNoPrompt(string message, Action<PopupExitResult> promptResultAction)
        {
            this._promptEnumResult = promptResultAction;
            this.SetDisplayData(message, PopupType.YesNo);
        }
        /// <summary>
        /// Display a card with a progress bar.
        /// </summary>
        /// <param name="message">Displayed message</param>
        /// <param name="progressMaxValue">Max reachable value of the progress</param>
        public void ShowProgress(string message, int progressMaxValue)
        {
            this._promptStringResult = null;
            this.SetDisplayData(message, PopupType.Progress);
            this.ProgressCurrentValue = 0;
            this.ProgressMaxValue     = progressMaxValue;
        }
        /// <summary>
        /// Hide the displayed progress bar card.
        /// </summary>
        public void HideProgress() =>
            this.SetReturnData(PopupExitResult.OK);
        #endregion

        #region Methods :: Button press exiting methods
        public void OnOK()
        {
            var returnedText = this.TypedInput;
            this.SetReturnData(PopupExitResult.OK);
            this._promptStringResult?.Invoke(Result<string, object>.MakeSuccess(returnedText));
        }
        public void OnCancel()
        {
            this.SetReturnData(PopupExitResult.Cancel);
            this._promptStringResult?.Invoke(Result<string, object>.MakeError());
        }
        public void OnYes()
        {
            this.SetReturnData(PopupExitResult.Yes);
            this._promptEnumResult?.Invoke(PopupExitResult.Yes);
        }
        public void OnNo()
        {
            this.SetReturnData(PopupExitResult.No);
            this._promptEnumResult?.Invoke(PopupExitResult.No);
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
