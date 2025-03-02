using MessagePipe;
using NodeChange;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class NodeChangeButton : MonoBehaviour
{
    [Inject] private IPublisher<NodeChangeRequest> _publisher;

    [SerializeField] private Button button;
    [SerializeField] private int chosenIndex;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log($"Button clicked, requesting scene change to {chosenIndex}");
            _publisher.Publish(new NodeChangeRequest(chosenIndex));
        });
    }
}
