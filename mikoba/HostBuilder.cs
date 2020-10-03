using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mikoba.CoreImplementations;
using Xamarin.Essentials;

namespace mikoba
{
    // public class MikobaAgent : AgentBase
    // {
    //     /// <summary>
    //     /// Initializes a new instance of the <see cref="T:AgentFramework.Core.Handlers.Agents.DefaultAgent"/> class.
    //     /// </summary>
    //     /// <param name="provider">Provider.</param>
    //     public MikobaAgent(IServiceProvider provider) : base(provider)
    //     {
    //         Console.WriteLine("Hello!");
    //     }
    //
    //     /// <summary>
    //     /// Configures the handlers.
    //     /// </summary>
    //     protected override void ConfigureHandlers()
    //     {
    //         base.AddConnectionHandler();
    //         base.AddDiscoveryHandler();
    //         base.AddBasicMessageHandler();
    //         base.AddForwardHandler();
    //         base.AddTrustPingHandler();
    //         base.AddCredentialHandler();
    //         
    //         // this.Handlers.Add(Provider.GetRequiredService<MikobaCredentialHandler>());
    //         this.Handlers.Add(Provider.GetRequiredService<MikobaProofHandler>());
    //     }
    //
    // }

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
                            options.AutoRespondCredentialOffer = true;
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
