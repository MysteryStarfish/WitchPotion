using TMPro;
using UnityEngine;
using WitchPotion.Bag;

public class ItemDescriptionFloatingPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;

    void OnEnable()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. ItemDescriptionFloatingPanel requires a Canvas to function.");
            return;
        }
        transform.SetParent(canvas.transform, true);
    }

    public void SetName(string itemName)
    {
        nameText.text = itemName;
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }

    public void SetItem(BagDisplayItem item)
    {
        SetName(item.Name);
        SetDescription(item.Description);
    }
}
