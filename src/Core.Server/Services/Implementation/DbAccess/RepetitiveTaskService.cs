using TrzyszczCMS.Core.Server.Models;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.TrzyszczCMS.Core.Server.Helpers;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess
{
    public class RepetitiveTaskService : IRepetitiveTaskService, IDisposable
    {
        #region Fields
        private readonly IDatabaseStrategy _databaseStrategy;
        private readonly RepetitiveTask _tokenRevocationTask;
        #endregion

        #region Ctor
        public RepetitiveTaskService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
            this._tokenRevocationTask = new(TimeSpan.FromMilliseconds(Constants.REPETITIVE_CYCLE_PERIOD_FOR_ACCESS_TOKEN_REVOKE_MILLIS))
            {
                Action = RevokingTokensHandle
            };
        }
        #endregion

        #region Methods
        public async Task StartRevokingTokensAsync() => await this._tokenRevocationTask.StartAsync();
        #endregion

        #region Timer handlers
        private async Task RevokingTokensHandle(Action disposeTimer)
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
                        disposeTimer.Invoke();
                    }
                    await ts.CommitAsync();
                }
            }
        }
        #endregion

        #region Dispose
        private bool _disposed = false;
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._tokenRevocationTask.Dispose();
            this._disposed = true;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
