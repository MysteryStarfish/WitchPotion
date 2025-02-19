using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    private Image image;
    private TMP_Text text;

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
}
