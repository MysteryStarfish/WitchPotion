using MessagePipe;

public class NodeChangePublisher
{
    private readonly IPublisher<NodeChangeRequest> _publisher;

    public NodeChangePublisher(IPublisher<NodeChangeRequest> publisher)
    {
        _publisher = publisher;
        // Debug.Log($"NodeChangePublisher subscribed successfully.");
    }

    public void RequestSceneChange(int chosenIndex)
    {
        // Debug.Log($"Publishing scene change request: {chosenIndex}");
        _publisher.Publish(new NodeChangeRequest(chosenIndex));
    }
}
