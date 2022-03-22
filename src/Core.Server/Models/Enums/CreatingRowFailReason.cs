namespace TrzyszczCMS.Core.Server.Models.Enums
{
    /// <summary>
    /// It says what caused not executing the creation.
    /// </summary>
    public enum CreatingRowFailReason
    {
        AlreadyExisting,
        CreatingForbidden,
    }
}
