using TrzyszczCMS.Core.Server.Helpers;
using TrzyszczCMS.Core.Server.Models;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess
{
    public class RepetitiveTaskService : IRepetitiveTaskService, IDisposable
    {
        #region Fields
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Fields :: Timers
        /// <summary>
        /// The timer that repetitively revokes tokens.
        /// </summary>
        private SemaphoredValue<Timer> _accessTokenRevocationTimer;
        #endregion

        #region Ctor
        public RepetitiveTaskService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
            this._accessTokenRevocationTimer = new SemaphoredValue<Timer>(() => null);
        }
        #endregion

        #region Methods
        public async Task StartRevokingTokensAsync()
        {
            var initiated = this.InstantiateTimer(
                this._accessTokenRevocationTimer,
                Constants.REPETITIVE_CYCLE_PERIOD_FOR_ACCESS_TOKEN_REVOKE_MILLIS,
                async (s, e) => await RevokingTokensHandle()
            );
            if (!initiated)
            {
                await RevokingTokensHandle();
            }
        }
        #endregion

        #region Timer handlers
        private async Task RevokingTokensHandle()
        {
            System.Diagnostics.Debug.WriteLine("RevokingTokensHandle");
            using (var ctx = this._databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var expiredTokens = ctx.AuthTokens.Where(i => i.UtcExpiryTime <= DateTime.UtcNow);
                    ctx.AuthTokens.RemoveRange(expiredTokens);
                    await ctx.SaveChangesAsync();
                    if (!await ctx.AuthTokens.AsNoTracking().AnyAsync())
                    {
                        this.DisposeTimer(this._accessTokenRevocationTimer);
                    }
                    await ts.CommitAsync();
                }
            }
        }
        #endregion

        #region Helper methods
        private bool InstantiateTimer(SemaphoredValue<Timer> timer, int millisPeriod, ElapsedEventHandler elapsedTimeHandler)
        {
            return timer.Synchronise(smp =>
            {
                if (smp.Invoke(i => i != null))
                {
                    return true;
                }
                System.Diagnostics.Debug.WriteLine("InstantiateTimer");
                var timerInstance = new Timer()
                {
                    AutoReset = true,
                    Interval = millisPeriod
                };
                timerInstance.Elapsed += elapsedTimeHandler;
                timerInstance.Enabled = true;
                smp.SetValue(timerInstance);
                return false;
            });
        }

        private void DisposeTimer(SemaphoredValue<Timer> timer)
        {
            timer.Synchronise(smp =>
            {
                System.Diagnostics.Debug.WriteLine("DisposeTimer");
                smp.Invoke(i =>
                {
                    i.Enabled = false;
                    i.Dispose();
                });
                smp.SetValue(null);
            });
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this._accessTokenRevocationTimer != null)
            {
                System.Diagnostics.Debug.WriteLine("Dispose");
                this._accessTokenRevocationTimer.Synchronise(smp =>
                {
                    System.Diagnostics.Debug.WriteLine("Dispose :: Synchronise");
                    smp.Invoke(i =>
                    {
                        i.Enabled = false;
                        i.Dispose();
                    });
                    smp.SetValue(null);
                });
                this._accessTokenRevocationTimer = null;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
