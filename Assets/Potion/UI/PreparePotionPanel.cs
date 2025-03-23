using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using WitchPotion.Bag;

public class PreparePotionPanel : MonoBehaviour
{
    [SerializeField]
    private PotionListItem potionListItemPrefab;
    [SerializeField]
    private Transform potionListContent;
    [SerializeField]
    private TMP_Text limitText;
    [SerializeField]
    private Button goButton;

    [Inject]
    private BagContext bagContext;

    private int maxCount = 10;
    private int currentCount = 0;

    void Start()
    {
        // 一階爆炸藥水
        this.bagContext.PotionBag.SetCount("1001", 10);
        foreach (var (potion, count) in this.bagContext.PotionBag.GetAll())
        {
            this.setupPotionListItem(potion, count);
        }
        this.goButton.onClick.AddListener(() =>
        {
            Debug.Log("Go button clicked");
            SceneManager.LoadScene("SampleScene");
        });
    }

    void Update()
    {
        this.limitText.text = $"Limit: {this.currentCount} / {this.maxCount}";
    }

    private void setupPotionListItem(Potion potion, int potionCount)
    {
        var potionListItem = Instantiate(potionListItemPrefab, potionListContent);
        potionListItem.Setup(potion.Sprite, potionCount);
        potionListItem.OnAddPotionButtonClicked.AddListener(() =>
        {
            if (this.currentCount >= this.maxCount)
            {
                Debug.Log("Global max count reached");
                return;
            }
            if (potionListItem.CurrentCount >= potionCount)
            {
                Debug.Log("Potion max count reached");
                return;
            }

            this.currentCount++;
            potionListItem.CurrentCount++;
        });
        potionListItem.OnRemovePotionButtonClicked.AddListener(() =>
        {
            if (this.currentCount <= 0)
            {
                Debug.Log("Global min count reached");
                return;
            }
            if (potionListItem.CurrentCount <= 0)
            {
                Debug.Log("Potion min count reached");
                return;
            }

            this.currentCount--;
            potionListItem.CurrentCount--;
        });
    }
}
