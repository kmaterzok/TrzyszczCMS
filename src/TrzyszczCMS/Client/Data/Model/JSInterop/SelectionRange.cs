namespace TrzyszczCMS.Client.Data.Model.JSInterop
{
    /// <summary>
    /// The range of values.
    /// </summary>
    public struct SelectionRange
    {
        /// <summary>
        /// Value that starts the range.
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Value that ends the range.
        /// </summary>
        public int End { get; set; }
        /// <summary>
        /// Is the span referring to the text snap or is it a cursor index.
        /// </summary>
        /// <returns></returns>
        public bool IsTextSelected() => this.Start < this.End;
        /// <summary>
        /// Returns length of selection. In other words - quantity of characters.
        /// </summary>
        /// <returns>Selection length</returns>
        public int GetLength() => this.End - this.Start;
    }
}
