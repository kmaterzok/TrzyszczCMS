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
        /// Get description about pixel size for Text wall module.
        /// </summary>
        /// <param name="width">Preferred width of a section block</param>
        /// <returns>Description of width in pixels</returns>
        public static string GetSectionWidthDescriptionAboutPixels(this TextWallSectionWidth width) =>
            $"{width.ToString()[1..]}px";

        /// <summary>
        /// Get description about viewport height of a heading banner.
        /// </summary>
        /// <param name="height">Desired banner height</param>
        /// <returns>Description of percentage height</returns>
        public static string GetPercentageDescription(this HeadingBannerHeight height) =>
            $"{height.ToString()[1..]}%";

        /// <summary>
        /// Get width of section for usage in the CSS style.
        /// </summary>
        /// <param name="width">Preferred width of a section block</param>
        /// <returns>Value of width for usage in the CSS style</returns>
        public static string GetCssStylePixelWidth(this TextWallSectionWidth width) =>
            $"{width.ToString()[1..]}px";

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
                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
        }
        /// <summary>
        /// Get human language translation of text wall part.
        /// </summary>
        /// <param name="type">Selected part</param>
        /// <returns>Human language translation (meaning)</returns>
        public static string GetTranslation(this TextWallEditedPart type)
        {
            switch (type)
            {
                case TextWallEditedPart.Section:    return "Section";
                case TextWallEditedPart.LeftAside:  return "Left extra";
                case TextWallEditedPart.RightAside: return "Right extra";
                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
        }
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
        public static PageManagementTool GetPageManagementTool(this TextWallEditedPart source)
        {
            switch (source)
            {
                case TextWallEditedPart.LeftAside:  return PageManagementTool.TextWallLeftAsideEditor;
                case TextWallEditedPart.RightAside: return PageManagementTool.TextWallRightAsideEditor;
                case TextWallEditedPart.Section:    return PageManagementTool.TextWallSectionEditor;
                default:
                    throw ExceptionMaker.Argument.Unsupported(source, nameof(source));
            }
        }
        /// <summary>
        /// Get <see cref="PageManagementTool"/> value based on a value of <see cref="PageModuleType"/>
        /// </summary>
        /// <param name="moduleType">Provided value to check</param>
        /// <returns>Value got by resolving input value</returns>
        public static PageManagementTool GetPageManagementTool(this PageModuleType moduleType)
        {
            switch (moduleType)
            {
                case PageModuleType.HeadingBanner: return PageManagementTool.HeadingBannerEditor;
                case PageModuleType.TextWall:
                    throw ExceptionMaker.Argument.Invalid(moduleType, nameof(moduleType));
                default:
                    throw ExceptionMaker.NotImplemented.ForHandling(moduleType, nameof(moduleType));
            }
        }
    }
}
