using VContainer;
using VContainer.Unity;

public class MapLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<MapController>(Lifetime.Singleton);
    }
}
