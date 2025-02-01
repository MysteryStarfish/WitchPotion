using System;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private MapViewor view;
    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();

        // 註冊 Publisher 和 Subscriber
        builder.Register<NodeChangePublisher>(Lifetime.Singleton);
        builder.Register<MapController>(Lifetime.Singleton);
        MapController.Instance.GenerateMap();
        builder.RegisterComponent(view);
        builder.Register<NodeChangeSubscriber>(Lifetime.Singleton);
    }
}

