using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyperledger.Aries.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mikoba.Services;
using Xamarin.Essentials;

namespace mikoba
{
    public class HostBuilder
    {
        public static IHostBuilder BuildHost(Assembly platformSpecific = null)
        {
            return XamarinHost.CreateDefaultBuilder<App>()
                .ConfigureServices((_, services) =>
                {
                    var rootPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var walletPath = Path.Combine(path1: rootPath, path2: ".indy_client", path3: "wallets");
                    var tailsPath = Path.Combine(path1: FileSystem.AppDataDirectory, path2: ".indy_client", path3: "tails");

                    services.AddAriesFramework(builder => builder.RegisterEdgeAgent<MikobaAgent>(
                        options: options =>
                        {
                            options.PoolName = "kiva-sandbox";
                            options.EndpointUri = AppConstant.EndpointUri;

                            options.WalletConfiguration.StorageConfiguration =
                                new WalletConfiguration.WalletStorageConfiguration
                                {
                                    Path = walletPath
                                };

                            options.WalletConfiguration.Id = "MobileWallet";
                            options.WalletCredentials.Key = "SecretWalletKey";

                            options.RevocationRegistryDirectory = tailsPath;
                        },
                        delayProvisioning: true));
                    services.AddSingleton<IPoolConfigurator, PoolConfigurator>();


                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterAssemblyModules(typeof(KernelModule).Assembly);
                    if (platformSpecific != null)
                    {
                        containerBuilder.RegisterAssemblyModules(platformSpecific);
                    }

                    containerBuilder.Populate(services);
                    App.Container = containerBuilder.Build();
                });
        }
    }
}
