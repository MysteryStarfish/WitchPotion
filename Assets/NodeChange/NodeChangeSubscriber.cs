using Map;
using UnityEngine;
using MessagePipe;

public class NodeChangeSubscriber
{
    private readonly ISubscriber<NodeChangeRequest> _subscriber;
    private readonly MapController _mapController;

    public NodeChangeSubscriber(ISubscriber<NodeChangeRequest> subscriber, MapController mapController)
    {
        _subscriber = subscriber;
        _mapController = mapController;
        // Debug.Log("SceneChangeSubscriber subscribed successfully.");
        _subscriber.Subscribe(OnNodeChangeRequested);
    }

    private void OnNodeChangeRequested(NodeChangeRequest request)
    {
        // Debug.Log($"Changing scene to: {request.ChosenIndex}");
        MapController.Instance.DoNextStep(request.ChosenIndex);
    }
}
