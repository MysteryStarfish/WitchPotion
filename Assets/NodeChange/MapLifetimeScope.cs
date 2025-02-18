using VContainer;
using VContainer.Unity;
using Map;
using UnityEngine;
using UpdateButtonViewRequest;

public class MapLifetimeScope : LifetimeScope
{
    [SerializeField] private MapViewer view;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(view);
        builder.Register<MapController>(Lifetime.Singleton);
        MapController.Instance.GenerateMap();


        // 註冊 Publisher 和 Subscriber
        builder.Register<UpdateButtonViewPublisher>(Lifetime.Singleton);
        builder.Register<UpdateButtonViewSubscriber>(Lifetime.Singleton);
        builder.Register<NodeChangePublisher>(Lifetime.Singleton);
        builder.Register<NodeChangeSubscriber>(Lifetime.Singleton);
    }
}
