using System;
using System.Threading.Tasks;
using System.Timers;
using TrzyszczCMS.Core.Server.Helpers;

namespace TrzyszczCMS.TrzyszczCMS.Core.Server.Helpers
{
    /// <summary>
    /// The task that is periodically executed until some conditions predefined conditions occur.
    /// </summary>
    public class RepetitiveTask : IDisposable
    {
        #region Fields :: Timers
        /// <summary>
        /// The timer that repetitively executes the task.
        /// </summary>
        private SemaphoredValue<Timer> _taskTimer;
        /// <summary>
        /// The timespan between each repetitiion of the task.
        /// </summary>
        private TimeSpan _repetitionPeriod;
        #endregion

        #region Ctor
        /// <summary>
        /// It prepares basic data of the repetitive task instance.
        /// </summary>
        /// <param name="period">The timespan between each repetition of the task</param>
        public RepetitiveTask(TimeSpan period)
        {
            this._taskTimer = new SemaphoredValue<Timer>(() => null);
            this._repetitionPeriod = period;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The action executed repeatedly.
        /// </summary>
        public RepetitiveActionCallback Action { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Start repetitive executing of <see cref="Action"/>. If not executing, it is started again.
        /// </summary>
        /// <remarks>
        /// This method is idempotent.
        /// </remarks>
        /// <returns>Task that starts the repetitiveness</returns>
        public async Task StartAsync()
        {
            var initiated = InstantiateTimer(
                this._repetitionPeriod,
                async (s, e) => await TimerRepetitiveActionHandle()
            );
            if (!initiated)
            {
                await TimerRepetitiveActionHandle();
            }
        }
        #endregion

        #region Timer handlers
        private async Task TimerRepetitiveActionHandle() => await this.Action.Invoke(this.DisposeTimer);
        #endregion

        #region Helper methods
        private bool InstantiateTimer(TimeSpan period, ElapsedEventHandler elapsedTimeHandler) => this._taskTimer.Synchronise(smp =>
        {
            if (smp.Invoke(i => i != null))
            {
                return true;
            }
            System.Diagnostics.Debug.WriteLine("InstantiateTimer");
            var timerInstance = new Timer()
            {
                AutoReset = true,
                Interval = period.TotalMilliseconds
            };
            timerInstance.Elapsed += elapsedTimeHandler;
            timerInstance.Enabled = true;
            smp.SetValue(timerInstance);
            return false;
        });

        private void DisposeTimer() => this._taskTimer.Synchronise(smp =>
        {
            System.Diagnostics.Debug.WriteLine("DisposeTimer");
            smp.Invoke(i =>
            {
                i.Enabled = false;
                i.Dispose();
            });
            smp.SetValue(null);
        });
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this._taskTimer != null)
            {
                System.Diagnostics.Debug.WriteLine("Dispose");
                this._taskTimer.Synchronise(smp =>
                {
                    System.Diagnostics.Debug.WriteLine("Dispose :: Synchronise");
                    smp.Invoke(i =>
                    {
                        i.Enabled = false;
                        i.Dispose();
                    });
                    smp.SetValue(null);
                });
                this._taskTimer = null;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
