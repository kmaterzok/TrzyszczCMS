using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Data.Model.JSInterop;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.Other.MarkDown;
using TrzyszczCMS.Client.Other.MarkDown.Strategies;

namespace TrzyszczCMS.Client.Views.Shared.Editors
{
    public partial class MarkDownEditor
    {
        #region Fields
        private ToggledMarkDownViewMode editorWorkMode = ToggledMarkDownViewMode.EditorWithPreview;
        private readonly MarkDownFormatter markDownFormatter = new MarkDownFormatter();
        #endregion

        #region Properties
        private string CssClassesForEditor =>
            CssClassesHelper.ClassesForMarkDownEditor(editorWorkMode).EditorCss;
        private string CssClassesForPreview =>
            CssClassesHelper.ClassesForMarkDownEditor(editorWorkMode).PreviewCss;
        private bool EditorButtonsDisabled =>
            editorWorkMode == ToggledMarkDownViewMode.Preview;
        #endregion

        #region Init
        protected override void OnInitialized()
        {
            this.markDownFormatter.GetSelectionRangeAsync = this.GetSelectionRangeAsync;
            this.markDownFormatter.SelectTextRangeAsync   = this.SelectTextRangeAsync;
            this.markDownFormatter.SelectTextIndexAsync   = this.SelectTextIndexAsync;
            base.OnInitialized();
        }
        #endregion

        #region Handles
        private void OnInputChanged(ChangeEventArgs args) =>
            this.markDownFormatter.MarkDownCode = args.Value.ToString();

        private void ToggleView()
        {
            editorWorkMode = editorWorkMode.NextValue();
        }

        private async Task AddFormattingAsterisks(AsteriskSuffixFormat format) =>
            await this.markDownFormatter.AddFormattingAsterisks(format);

        private async Task AddFormattingSuffixes([NotNull] string suffix) =>
            await this.markDownFormatter.AddFormattingSuffixes(suffix);

        private async Task AddFormattingSuffixesLeft(LeftSuffixType type)
        {
            var strategy = LeftSuffixFormatStrategyFactory.Make(type);
            await this.markDownFormatter.AddFormattingSuffixesLeft(strategy);
        }

        private async Task AddLinkBasedText(LinkBasedContentType type) =>
            await this.markDownFormatter.AddLinkBasedText(type);
        #endregion

        #region JSInterop
        private async Task<SelectionRange> GetSelectionRangeAsync() =>
            await this.JSInterop.GetSelectionRangeAsync("txarMarkDownEditedText");
        
        private async Task SelectTextIndexAsync(int index) =>
            await this.JSInterop.SelectTextIndexAsync("txarMarkDownEditedText", index, this.StateHasChanged);

        private async Task SelectTextRangeAsync(int start, int end) =>
            await this.JSInterop.SelectTextRangeAsync("txarMarkDownEditedText", start, end, this.StateHasChanged);
        #endregion
    }
}