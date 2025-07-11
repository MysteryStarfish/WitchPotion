using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VContainer;
using WitchPotion.Bag;

public class ItemCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler, IPointerMoveHandler, IPointerEnterHandler
{
    private string key;
    private Image image;
    private TMP_Text text;

    private BagDisplayItem bagDisplayItem;
    private GameObject draggingObject;
    [Inject]
    private ItemDescriptionFloatingPanel itemDescriptionPanel;

    private void Awake()
    {
        this.image = GetComponentInChildren<Image>();
        this.text = GetComponentInChildren<TMP_Text>();
        this.key = "";
    }

    public void SetItem(Sprite sprite, int count, string key)
    {
        this.image.color = Color.white;
        this.image.sprite = sprite;
        this.text.text = count.ToString();
        this.key = key;
    }

    public void SetItem(BagDisplayItem item, int count, string key)
    {
        this.image.color = Color.white;
        this.image.sprite = item.Sprite;
        this.text.text = count.ToString();
        this.key = key;
        this.bagDisplayItem = item;
    }

    public void RemoveItem()
    {
        this.image.color = Color.gray;
        this.image.sprite = null;
        this.text.text = string.Empty;
        this.key = "";
        this.bagDisplayItem = null;
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
        if (this.draggingObject == null)
        {
            return;
        }

        RaycastHit2D raycastHit2D = Physics2D.Raycast(eventData.position, Vector2.zero);
        if (raycastHit2D.collider != null)
        {
            Debug.Log($"OnEndDrag: {raycastHit2D.collider.name}");
            var handler = raycastHit2D.collider.GetComponent<OnHerbDroppedHandler>();
            if (handler != null)
            {
                handler.OnHerbDropped(this.key);
            }
        }
        Destroy(this.draggingObject);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.bagDisplayItem == null)
        {
            return;
        }

        this.itemDescriptionPanel.gameObject.SetActive(true);
        this.itemDescriptionPanel.transform.position = eventData.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.bagDisplayItem == null)
        {
            Debug.LogWarning("OnPointerExit: No bag display item set.");
            return;
        }
        this.itemDescriptionPanel.gameObject.SetActive(false);
        Debug.Log($"OnPointerExit: {eventData.position}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.bagDisplayItem == null)
        {
            Debug.LogWarning("OnPointerEnter: No bag display item set.");
            return;
        }

        this.itemDescriptionPanel.gameObject.SetActive(true);
        this.itemDescriptionPanel.SetItem(this.bagDisplayItem);
    }
}
