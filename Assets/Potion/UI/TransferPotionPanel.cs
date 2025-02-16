using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using WitchPotion.Bag;

public class TransferPotionPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private TMP_InputField countInput;
    [SerializeField]
    private Button transferButton;

    [Inject]
    private BagContext bagContext;
    private PotionBag potionBag => this.bagContext.PotionBag;
    private PotionBag playerPotionBag => this.bagContext.PlayerPotionBag;

    void Start()
    {
        this.potionBag.SetCount("爆炸藥水", 10);

        this.transferButton.onClick.AddListener(() =>
        {
            string name = this.nameInput.text;
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Empty name");
                return;
            }
            if (!int.TryParse(this.countInput.text, out int requiredCount))
            {
                Debug.LogWarning("Invalid count");
                return;
            }
            if (requiredCount <= 0)
            {
                Debug.LogWarning("count <= 0");
                return;
            }

            // HACK: hard-coded mapping
            if (name == "boom")
            {
                name = "爆炸藥水";
            }

            var currentCount = this.potionBag.GetCount(name);
            if (currentCount < requiredCount)
            {
                Debug.LogWarning("Not enough potion");
                return;
            }
            this.potionBag.SetCount(name, currentCount - requiredCount);
            var currentPlayerCount = this.playerPotionBag.GetCount(name);
            this.playerPotionBag.SetCount(name, currentPlayerCount + requiredCount);
        });
    }
}
