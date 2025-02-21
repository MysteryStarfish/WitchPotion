using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PotionListItem : MonoBehaviour
{
    [SerializeField]
    private Button addPotionButton;
    [SerializeField]
    private Button removePotionButton;
    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private Image potionImage;

    public int CurrentCount;
    private int maxCount;

    public void Setup(Sprite sprite, int maxCount)
    {
        this.potionImage.sprite = sprite;
        this.maxCount = maxCount;
        this.CurrentCount = 0;
        this.countText.text = $"{this.CurrentCount} / {this.maxCount}";
    }

    public UnityEvent OnAddPotionButtonClicked => addPotionButton.onClick;
    public UnityEvent OnRemovePotionButtonClicked => removePotionButton.onClick;

    public void Update()
    {
        this.countText.text = $"{this.CurrentCount} / {this.maxCount}";
    }
}
