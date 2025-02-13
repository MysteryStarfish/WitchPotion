using System;
using Map;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using UnityEngine;
using UpdateButtonViewRequest;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private MapViewer view;
    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();

        builder.Register<UpdateButtonViewPublisher>(Lifetime.Singleton);
        builder.RegisterComponent(view);
        builder.Register<UpdateButtonViewSubscriber>(Lifetime.Singleton);
        // 註冊 Publisher 和 Subscriber
        builder.Register<NodeChangePublisher>(Lifetime.Singleton);
        builder.Register<MapController>(Lifetime.Singleton);
        MapController.Instance.GenerateMap();
        builder.Register<NodeChangeSubscriber>(Lifetime.Singleton);
        
    }
}

