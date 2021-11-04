using Core.Shared.Enums;
using Core.Shared.Models.PageContent;
using DAL.Models.Database.Tables;

namespace Core.Server.Models.Extensions
{
    /// <summary>
    /// Methods extending models functionality.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Create a complementary instance of <see cref="ModuleContent"/> basing on <see cref="Cont_TextWallModule"/> instance.
        /// </summary>
        /// <param name="dbTextWallModule">Input instance</param>
        /// <returns>Output instance</returns>
        public static ModuleContent ToModuleContent(this Cont_TextWallModule dbTextWallModule)
        {
            return new ModuleContent()
            {
                TextWallModuleContent = new TextWallModuleContent()
                {
                    LeftAsideMarkDownContent = dbTextWallModule.LeftAsideContent,
                    RightAsideMarkDownContent = dbTextWallModule.RightAsideContent,
                    SectionMarkDownContent = dbTextWallModule.SectionContent,
                    SectionWidth = (TextWallSectionWidth)dbTextWallModule.SectionWidth
                }
            };

        }
    }
}
