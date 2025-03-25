using Map.PotionButton;
using MessagePipe;
using NodeChange;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class NodeChangeButton : MonoBehaviour, IDropHandler
{
    [Inject] private IPublisher<NodeChangeRequest> _nodeChangeRequestPublisher;
    [Inject] private IPublisher<UsePotionRemoveObstacleRequest> _usePotionRemoveObstacleRequestPublisher;

    [SerializeField] private Button button;
    [SerializeField] private int chosenIndex;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log($"Button clicked, requesting scene change to {chosenIndex}");
            _nodeChangeRequestPublisher.Publish(new NodeChangeRequest(chosenIndex));
        });
    }

    public void OnDrop(PointerEventData eventData)
    {
        string code = eventData.pointerDrag.GetComponent<PotionButton>().potion.code;
        _usePotionRemoveObstacleRequestPublisher.Publish(new UsePotionRemoveObstacleRequest(code, chosenIndex));
    }
}
