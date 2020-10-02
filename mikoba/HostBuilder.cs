using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyperledger.Aries.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddAriesFramework(builder => builder.RegisterEdgeAgent(
                        options: options =>
                        {
                            options.EndpointUri = AppConstant.EndpointUri;
                            options.AutoRespondCredentialOffer = false;
                            options.AutoRespondCredentialRequest = true;    
                            options.WalletConfiguration.StorageConfiguration =
                                new WalletConfiguration.WalletStorageConfiguration
                                {
                                    Path = Path.Combine(
                                        path1: FileSystem.AppDataDirectory,
                                        path2: ".indy_client",
                                        path3: "wallets")
                                };
                            options.WalletConfiguration.Id = "MobileWallet";
                            options.WalletCredentials.Key = "SecretWalletKey";
                            options.RevocationRegistryDirectory = Path.Combine(
                                path1: FileSystem.AppDataDirectory,
                                path2: ".indy_client",
                                path3: "tails");
                        },
                        delayProvisioning: true));

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
