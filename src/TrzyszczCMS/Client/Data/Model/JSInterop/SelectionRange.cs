using System;

namespace TrzyszczCMS.Client.Data.Model.JSInterop
{
    /// <summary>
    /// The range of values.
    /// </summary>
    public struct SelectionRange
    {
        #region Properties
        /// <summary>
        /// Value that starts the range.
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Value that ends the range.
        /// </summary>
        public int End { get; set; }
        #endregion

        #region Ctor
        public SelectionRange(int start, int end)
        {
            this.Start = start;
            this.End   = end;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Is the span referring to the text snap or is it a cursor index.
        /// </summary>
        /// <returns></returns>
        public bool IsTextSelected()
        {
            this.EnsureEndGreaterOrEqualToStart();
            return this.Start < this.End;
        }
        /// <summary>
        /// Returns length of selection. In other words - quantity of characters.
        /// </summary>
        /// <returns>Selection length</returns>
        public int GetLength()
        {
            this.EnsureEndGreaterOrEqualToStart();
            return this.End - this.Start;
        }
        /// <summary>
        /// Ensure if <see cref="Start"/> and <see cref="End"/> fit to each other in the terms of equality.
        /// </summary>
        private void EnsureEndGreaterOrEqualToStart()
        {
            if (this.Start > this.End)
            {
                throw new InvalidOperationException("The end value must be greater or equal to the start value.");
            }
        }
        #endregion
    }
}
