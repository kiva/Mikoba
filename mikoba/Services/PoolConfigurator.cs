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

        private Dictionary<string, string> poolConfigs = new Dictionary<string, string>
        {
            { "kiva-sandbox", "pool_kiva_sandbox" }
        };

        private readonly ILogger<PoolConfigurator> logger;

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
                    var rootPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var cache = FileSystem.CacheDirectory;
                    var filename = Path.Combine(rootPath, "genesis.txn");

                    // Dump file contents to cached filename
                    using (var stream = await FileSystem.OpenAppPackageFileAsync(config.Value))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var contents = await reader.ReadToEndAsync().ConfigureAwait(false);
                            File.WriteAllText(filename, contents);
                        }
                    }

                    // Create pool configuration
                    await poolService.CreatePoolAsync(config.Key, filename)
                        .ConfigureAwait(false);
                }
                catch (PoolLedgerConfigExistsException ex)
                {
                    // OK
                    Tracking.TrackException(ex, "Pool already exists");
                }
                catch (Exception ex)
                {
                    Tracking.TrackException(ex,"Couldn't create pool");
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
