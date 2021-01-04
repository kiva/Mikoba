using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hyperledger.Aries.Contracts;
using Hyperledger.Indy.PoolApi;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using Sentry.Protocol;
using Xamarin.Essentials;

namespace mikoba.Services
{
    public class PoolConfigurator : IPoolConfigurator, IHostedService
    {
        private readonly IPoolService poolService;
        private readonly ILogger<PoolConfigurator> logger;

        private Dictionary<string, string> poolConfigs = new Dictionary<string, string>
        {
            { "kiva", "pool_kiva_sandbox" },
        };

        public PoolConfigurator(
            IPoolService poolService,
            ILogger<PoolConfigurator> logger)
        {
            this.poolService = poolService;
            this.logger = logger;
        }

        public async Task ConfigurePoolsAsync()
        {
            foreach (var config in poolConfigs)
            {
                try
                {
                    // Path for bundled genesis txn
                    var filename = Path.Combine(FileSystem.CacheDirectory, "genesis.txn");

                    // Dump file contents to cached filename
                    using (var stream = await FileSystem.OpenAppPackageFileAsync(config.Value))
                    using (var reader = new StreamReader(stream))
                    {
                        File.WriteAllText(filename, await reader.ReadToEndAsync()
                            .ConfigureAwait(false));
                    }

                    // Create pool configuration
                    await poolService.CreatePoolAsync(config.Key, filename)
                        .ConfigureAwait(false);
                }
                catch (PoolLedgerConfigExistsException ex)
                {
                    // OK
                    logger.LogCritical(ex, "Pool already exists");
                    Crashes.TrackError(ex);
                    SentrySdk.CaptureException(ex);
                    
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Couldn't create pool config");
                    Crashes.TrackError(ex);
                    SentrySdk.CaptureException(ex);
                    SentrySdk.CaptureMessage("Couldn't create pool", SentryLevel.Error);
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return ConfigurePoolsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public interface IPoolConfigurator
    {
        Task ConfigurePoolsAsync();
    }
}
