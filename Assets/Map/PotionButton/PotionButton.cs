using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Map.PotionButton
{
    public class PotionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] public Potion potion;
        [SerializeField] private int leftAmount;

        private RectTransform _rectTransform;
        private Vector3 _originalRectPosition;
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Image image = GetComponent<Image>();
            image.sprite = potion.sprite;
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalRectPosition = _rectTransform.position;
            _canvasGroup.blocksRaycasts = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _rectTransform.position = _originalRectPosition;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}