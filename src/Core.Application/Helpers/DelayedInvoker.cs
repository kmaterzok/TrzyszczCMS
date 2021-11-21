using System;
using System.Timers;

namespace Core.Application.Helpers
{
    /// <summary>
    /// The Timer-based invoker for delayed invoke of action.
    /// </summary>
    public class DelayedInvoker : IDisposable
    {
        #region Fields
        /// <summary>
        /// Counts down time and invokes <seealso cref="_action"/>.
        /// </summary>
        private Timer _timer;
        /// <summary>
        /// Invoked when the set time was elapsed from the last invoke of <seealso cref="Invoke"/>.
        /// </summary>
        private readonly Action _action;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="awaitTimeMs">Time between the latest invokation of <see cref="Invoke"/> and the invoking of <paramref name="action"/>.</param>
        /// <param name="action">Invoked when <paramref name="awaitTimeMs"/>ms time up.</param>
        public DelayedInvoker(int awaitTimeMs, Action action)
        {
            this._timer = new Timer()
            {
                Interval = awaitTimeMs,
                Enabled = false
            };
            this._timer.Elapsed += OnTimerElapsed;
            this._action = action;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Invoke the defined action.
        /// </summary>
        public void Invoke()
        {
            this._timer.Enabled = false;
            this._timer.Enabled = true;
        }
        #endregion

        #region Private methods
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._action.Invoke();
            this._timer.Enabled = false;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this._timer != null)
            {
                this._timer.Dispose();
                this._timer = null;
            }
        }
        ~DelayedInvoker() => Dispose();
        #endregion
    }
}
