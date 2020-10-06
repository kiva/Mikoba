using Autofac;
using Hyperledger.Aries.Agents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using mikoba.CoreImplementations;
using mikoba.Services;

namespace mikoba
{
    public class KernelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<NavigationService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<MikobaCredentialHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();            
            
            builder
                .RegisterType<PoolConfigurator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<MikobaProofHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<ActionDispatcher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<KeyValueStoreService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<MediatorTimerService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            var loggerFactory = LoggerFactory.Create(logBuilder => { logBuilder.AddConsole().AddDebug(); });
            builder
                .RegisterInstance(loggerFactory)
                .As<ILoggerFactory>()
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("mikoba.ViewModels"))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("mikoba.UI"))
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("mikoba.CoreImplementations")).SingleInstance();
        }
    }
}
