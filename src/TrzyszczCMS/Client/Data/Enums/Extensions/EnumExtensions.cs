using Core.Application.Enums;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using System;
using System.Linq;

namespace TrzyszczCMS.Client.Data.Enums.Extensions
{
    /// <summary>
    /// Adds methods for enums. Eases usage of enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get complementary style for a specific width of text section.
        /// </summary>
        /// <param name="width">Preferred width of section block</param>
        /// <returns>Style name from isolated CSS file</returns>
        public static string SectionWidthCssClass(this TextWallSectionWidth width) =>
            $"section-width-{width.ToString()[1..]}";

        /// <summary>
        /// Get complementary style for a specific aside note.
        /// </summary>
        /// <param name="width">Preferred width of section block</param>
        /// <param name="leftNote"><c>false</c> - left note, <c>true</c> - right note</param>
        /// <returns>Style name from isolated CSS file</returns>
        public static string AsideInnerNoteCssClass(this TextWallSectionWidth width, bool leftNote)
        {
            var direction = leftNote ? "left" : "right";
            return $"aside-inner-note-{direction}-{width.ToString()[1..]}";
        }

        /// <summary>
        /// Get description about pixel size for Text wall module.
        /// </summary>
        /// <param name="width">Preferred width of a section block</param>
        /// <returns>Description of width in pixels</returns>
        public static string GetSectionWidthDescriptionAboutPixels(this TextWallSectionWidth width) =>
            $"{width.ToString()[1..]}px";

        /// <summary>
        /// Get description about pixel size for Post listing module.
        /// </summary>
        /// <param name="width">Preferred width of a listing block</param>
        /// <returns>Description of width in pixels</returns>
        public static string GetWidthDescriptionAboutPixels(this PostListingWidth width) =>
            $"{width.ToString()[1..]}px";

        /// <summary>
        /// Get width of section for usage in the CSS style.
        /// </summary>
        /// <param name="width">Preferred width of a section block</param>
        /// <returns>Value of width for usage in the CSS style</returns>
        public static string GetCssStylePixelWidth(this TextWallSectionWidth width) =>
            $"{width.ToString()[1..]}px";

        /// <summary>
        /// Get width of post listing for usage in the CSS style.
        /// </summary>
        /// <param name="width">Preferred width</param>
        /// <returns>Value of width for usage in the CSS style</returns>
        public static string GetCssStylePixelWidth(this PostListingWidth width) =>
            $"{width.ToString()[1..]}px";

        /// <summary>
        /// Get description about viewport height of a heading banner.
        /// </summary>
        /// <param name="height">Desired banner height</param>
        /// <returns>Description of percentage height</returns>
        public static string GetPercentageDescription(this HeadingBannerHeight height) =>
            $"{height.ToString()[1..]}%";

        /// <summary>
        /// Get value of the banner height as CSS value.
        /// </summary>
        /// <param name="height">Banner's Height</param>
        /// <returns>CSS value of banner's height.</returns>
        public static string GetBannerHeightCssValue(this HeadingBannerHeight height) =>
            $"{height.ToString()[1..]}vh";

        /// <summary>
        /// Get human language translation of module type enum.
        /// </summary>
        /// <param name="type">Selected module type</param>
        /// <returns>Human language translation (meaning)</returns>
        public static string GetTranslation(this PageModuleType type)
        {
            switch (type)
            {
                case PageModuleType.TextWall:      return "Text wall";
                case PageModuleType.HeadingBanner: return "Heading banner";
                case PageModuleType.PostListing:   return "Post listing";
                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
        }
        /// <summary>
        /// Get human language translation of text wall part.
        /// </summary>
        /// <param name="type">Selected part</param>
        /// <returns>Human language translation (meaning)</returns>
        public static string GetTranslation(this TextWallEditedPart type) => type switch
        {
            TextWallEditedPart.Section    => "Section",
            TextWallEditedPart.LeftAside  => "Left extra",
            TextWallEditedPart.RightAside => "Right extra",
            _ => throw ExceptionMaker.Argument.Unsupported(type, nameof(type))
        };
        
        /// <summary>
        /// Get human language translation of fail reason.
        /// </summary>
        /// <param name="type">Selected fail reason</param>
        /// <returns>Human language translation (meaning)</returns>
        public static string GetTranslation(this PasswordNotChangedReason reason) => reason switch
        {
            PasswordNotChangedReason.NewPasswordEqualsOldPassword => "The old password and the new password are same.",
            PasswordNotChangedReason.NewPasswordNotComplexEnough  => "The new password is not complex enough.",
            PasswordNotChangedReason.NotAllDataProvided           => "Not all necessary data has been provided.",
            PasswordNotChangedReason.OldPasswordInvalid           => "The old password is invalid.",
            PasswordNotChangedReason.RepeatedPasswordInvalid      => "The repeated password is invalid.",

            _ => throw ExceptionMaker.Argument.Unsupported(reason, nameof(reason))
        };
        
        /// <summary>
        /// Get value of next enum defined in <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of iterated enum</typeparam>
        /// <param name="source">Predcessor of the desired value</param>
        /// <returns>Desired value</returns>
        public static T NextValue<T>(this T source) where T : Enum
        {
            var values = Enum.GetValues(typeof(T))
                             .Cast<T>()
                             .ToList();

            var foundIndex = values.IndexOf(source);
            var nextValueIndex = (foundIndex + 1) % values.Count;

            return values[nextValueIndex];
        }
        /// <summary>
        /// Get the adequate tool enum for a specified <paramref name="source"/> argument.
        /// </summary>
        /// <param name="source">Type of the edited text wall part</param>
        /// <returns>Type of tool that handles managing the specified text wall's part.</returns>
        public static PageManagementTool GetPageManagementTool(this TextWallEditedPart source) => source switch
        {
            TextWallEditedPart.LeftAside  => PageManagementTool.TextWallLeftAsideEditor,
            TextWallEditedPart.RightAside => PageManagementTool.TextWallRightAsideEditor,
            TextWallEditedPart.Section    => PageManagementTool.TextWallSectionEditor,

            _ => throw ExceptionMaker.Argument.Unsupported(source, nameof(source))
        };
        
        /// <summary>
        /// Get <see cref="PageManagementTool"/> value based on a value of <see cref="PageModuleType"/>
        /// </summary>
        /// <param name="moduleType">Provided value to check</param>
        /// <returns>Value got by resolving input value</returns>
        public static PageManagementTool GetPageManagementTool(this PageModuleType moduleType) => moduleType switch
        {
            PageModuleType.HeadingBanner => PageManagementTool.HeadingBannerEditor,
            PageModuleType.PostListing   => PageManagementTool.PostListingEditor,
            PageModuleType.TextWall      => throw ExceptionMaker.Argument.Invalid(moduleType, nameof(moduleType)),

            _ => throw ExceptionMaker.NotImplemented.ForHandling(moduleType, nameof(moduleType))
        };
    }
}
