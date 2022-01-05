using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models.PageContent;
using DAL.Models.Database;
using System.Collections.Generic;
using System.Linq;

namespace Core.Server.Models.Extensions
{
    /// <summary>
    /// Methods extending models functionality.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Create a complementary instance of <see cref="ModuleContent"/> basing on <see cref="ContTextWallModule"/> instance.
        /// </summary>
        /// <param name="dbTextWallModule">Input instance</param>
        /// <returns>Output instance</returns>
        public static ModuleContent ToModuleContent(this ContTextWallModule dbTextWallModule) => new ModuleContent()
        {
            TextWallModuleContent = new TextWallModuleContent()
            {
                LeftAsideMarkDownContent = dbTextWallModule.LeftAsideContent,
                RightAsideMarkDownContent = dbTextWallModule.RightAsideContent,
                SectionMarkDownContent = dbTextWallModule.SectionContent,
                SectionWidth = (TextWallSectionWidth)dbTextWallModule.SectionWidth
            }
        };

        /// <summary>
        /// Create a complementary instance of <see cref="ContModule"/> basing on <see cref="ModuleContent"/> instance.
        /// </summary>
        /// <param name="source">Input instance</param>
        /// <returns>Output instance</returns>
        public static ContModule ToContModule(this ModuleContent source)
        {
            var type = source.GetModuleType();
            switch (type)
            {
                case PageModuleType.TextWall:
                    return new ContModule()
                    {
                        Type = (byte)PageModuleType.TextWall,
                        ContTextWallModule = new ContTextWallModule()
                        {
                            SectionWidth = (short)source.TextWallModuleContent.SectionWidth,
                            LeftAsideContent = source.TextWallModuleContent.LeftAsideMarkDownContent,
                            RightAsideContent = source.TextWallModuleContent.RightAsideMarkDownContent,
                            SectionContent = source.TextWallModuleContent.SectionMarkDownContent
                        }
                    };

                default:
                    throw ExceptionMaker.Argument.Unsupported(type, $"{nameof(source)}.{nameof(source.GetModuleType)}()");
            }
        }

        /// <summary>
        /// Create <see cref="List{Cont_Module}"/> instance by mapping data.
        /// </summary>
        /// <param name="source">Data to be mapped to desired list</param>
        /// <returns>Remapped list</returns>
        public static List<ContModule> ToContModulesList(this IEnumerable<ModuleContent> source) =>
            source?.Select(i => i.ToContModule()).ToList() ?? new List<ContModule>();
    }
}
