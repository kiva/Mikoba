using Autofac;
using mikoba;


namespace mikoba.Droid
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new KernelModule());
        }
    }
}
