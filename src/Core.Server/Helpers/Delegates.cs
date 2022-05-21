using System;
using System.Threading.Tasks;

namespace TrzyszczCMS.TrzyszczCMS.Core.Server.Helpers
{
    /// <summary>
    /// Callback of invoking the repetitive task's action.
    /// </summary>
    /// <param name="disposeTimer">Action for disposing the timer that counts time for a next invocation</param>
    /// <returns>Task executing the action</returns>
    public delegate Task RepetitiveActionCallback(Action disposeTimer);
}
