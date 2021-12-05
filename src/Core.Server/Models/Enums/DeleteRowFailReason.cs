namespace Core.Server.Models.Enums
{
    /// <summary>
    /// It says what caused not executing the deletion.
    /// </summary>
    public enum DeleteRowFailReason
    {
        NotFound,
        DeletingForbidden
    }
}
