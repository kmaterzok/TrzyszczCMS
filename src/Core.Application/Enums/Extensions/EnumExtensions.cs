using Core.Shared.Enums;
using System;

namespace Core.Application.Enums.Extensions
{
    /// <summary>
    /// The extension methods for enum types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get complementary style for a specific width of text section.
        /// </summary>
        /// <param name="width">Preferred width of section block</param>
        /// <returns>Style name from isolated CSS file</returns>
        public static string SectionWidthCssClass(this TextWallSectionWidth width)
        {
            switch (width)
            {
                case TextWallSectionWidth._600:  return "section-width-600";
                case TextWallSectionWidth._800:  return "section-width-800";
                case TextWallSectionWidth._1000: return "section-width-1000";
                case TextWallSectionWidth._1200: return "section-width-1200";
                default:
                    throw new ArgumentOutOfRangeException(nameof(width), "The argument is unable to be handled.");
            }
        }
    }
}
