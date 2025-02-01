using UnityEngine;
using MessagePipe;

public class NodeChangeSubscriber
{
    private readonly ISubscriber<NodeChangeRequest> _subscriber;
    private readonly MapController _mapController;
    private readonly MapViewor _view;

    public NodeChangeSubscriber(ISubscriber<NodeChangeRequest> subscriber, MapController mapController, MapViewor view)
    {
        _subscriber = subscriber;
        _mapController = mapController;
        _view = view;
        Debug.Log("SceneChangeSubscriber subscribed successfully.");
        _subscriber.Subscribe(OnNodeChangeRequested);
    }

    private void OnNodeChangeRequested(NodeChangeRequest request)
    {
        Debug.Log($"Changing scene to: {request.ChosenIndex}");
        MapController.Instance.DoNextStep(request.ChosenIndex);
        _view.UpdateButtonText();
    }
}
