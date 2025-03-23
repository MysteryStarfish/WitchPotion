using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Image image;
    private TMP_Text text;

    private GameObject draggingObject;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public void SetItem(Sprite sprite, int count)
    {
        image.color = Color.white;
        image.sprite = sprite;
        text.text = count.ToString();
    }

    public void RemoveItem()
    {
        image.color = Color.gray;
        image.sprite = null;
        text.text = string.Empty;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.image.sprite == null)
        {
            Debug.Log("OnBeginDrag: No item");
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        this.draggingObject = new GameObject("DraggingObject");
        this.draggingObject.transform.SetParent(canvas.transform, false);
        var draggingImage = this.draggingObject.AddComponent<Image>();
        draggingImage.sprite = this.image.sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.draggingObject == null)
        {
            return;
        }
        this.draggingObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.draggingObject != null)
        {
            Destroy(this.draggingObject);
        }
    }
}
