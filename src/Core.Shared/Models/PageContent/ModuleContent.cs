
using Core.Shared.Enums;
using Core.Shared.Exceptions;
using System;

namespace Core.Shared.Models.PageContent
{
    /// <summary>
    /// The content of a single module that is embedded into the displayed page.
    /// </summary>
    public class ModuleContent
    {
        #region Properties
        /// <summary>
        /// Reference on the module content of type <see cref="PageModuleType.TextWall"/>.
        /// If <c>null</c> - the module's type does not apply to the content.
        /// </summary>
        public TextWallModuleContent TextWallModuleContent { get; set; }
        #endregion

        #region Ctor
        public ModuleContent()
        {
            this.NullifyContentHolders();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determine the type of the page module.
        /// </summary>
        /// <returns>Page module enum value</returns>
        public PageModuleType GetModuleType()
        {
            if (this.TextWallModuleContent != null)
            {
                return PageModuleType.TextWall;
            }
            else
            {
                throw new InvalidMemberException($"The page type is unknown.");
            }
        }


        /// <summary>
        /// Set module data object.
        /// </summary>
        /// <typeparam name="T">The type of the set object</typeparam>
        /// <param name="module">The module that is set</param>
        public void SetModule<T>(T module)
        {
            this.NullifyContentHolders();

            if (typeof(T) == typeof(TextWallModuleContent))
            {
                this.TextWallModuleContent = (TextWallModuleContent)(object)module;
            }
            else
            {
                throw new ArgumentException("The assigned object is of unsupported class type.", nameof(module));
            }
        }

        /// <summary>
        /// Clear all references of data holders.
        /// </summary>
        private void NullifyContentHolders()
        {
            this.TextWallModuleContent = null;
        }
        #endregion
    }
}
