namespace DAL.Enums
{
    /// <summary>
    /// Type of accessed database depending on the load factor.
    /// </summary>
    public enum ConnectionStringDbType
    {
        /// <summary>
        /// Database that lets read data only.
        /// </summary>
        Read,
        /// <summary>
        /// Database that lets read and modify data.
        /// </summary>
        Modify
    }
}
