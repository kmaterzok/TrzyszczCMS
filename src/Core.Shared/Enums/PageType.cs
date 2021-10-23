namespace Core.Shared.Enums
{
    /// <summary>
    /// The precised type of page to be presented
    /// </summary>
    public enum PageType : byte
    {
        /// <summary>
        /// The top page of the whole website.
        /// </summary>
        HomePage = 1,
        /// <summary>
        /// The type of page used for long text and much content do express.
        /// It does not count as blog or diary post as its creation date is not very relevant.
        /// </summary>
        Article = 2,
        /// <summary>
        /// The type of page used for blog posts and text content mainly.
        /// Usually used for news and data which the creation date is relevant for.
        /// </summary>
        Post = 3
    }
}
