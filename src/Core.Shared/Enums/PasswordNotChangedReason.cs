namespace TrzyszczCMS.Core.Shared.Enums
{
    /// <summary>
    /// The reason why the password was not changed during the change trial.
    /// </summary>
    public enum PasswordNotChangedReason
    {
        NewPasswordNotComplexEnough,
        NewPasswordEqualsOldPassword,
        NotAllDataProvided,
        OldPasswordInvalid,
        RepeatedPasswordInvalid
    }
}
