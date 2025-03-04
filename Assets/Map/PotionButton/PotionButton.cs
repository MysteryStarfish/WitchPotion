using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Map.PotionButton
{
    public class PotionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Potion type;
        [SerializeField] private int _leftAmount;
        
        [Inject] private IPublisher<UsePotionRemoveObstacleRequest> _publisher;

        private RectTransform _rectTransform;
        private Vector3 _originalRectPosition;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Debug.Log(_originalRectPosition);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalRectPosition = _rectTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _rectTransform.position = _originalRectPosition;
            _publisher.Publish(new UsePotionRemoveObstacleRequest(type.potionName));
        }
    }
}