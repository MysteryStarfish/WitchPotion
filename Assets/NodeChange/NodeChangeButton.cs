using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class NodeChangeButton : MonoBehaviour
{
    [Inject] private NodeChangePublisher _publisher;
    [Inject] private NodeChangeSubscriber _subscriber;

    [SerializeField] private Button button;
    [SerializeField] private int chosenIndex;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log($"Button clicked, requesting scene change to {chosenIndex}");
            _publisher.RequestSceneChange(chosenIndex);
        });
    }
}
