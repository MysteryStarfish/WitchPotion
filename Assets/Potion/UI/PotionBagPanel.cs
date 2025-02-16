using System.Text;
using TMPro;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class PotionBagPanel : MonoBehaviour
{
    [SerializeField]
    private bool usePlayer;
    [SerializeField]
    private string header = "Potion Bag";
    [SerializeField]
    private TMP_Text text;

    [Inject]
    private BagContext bagContext;

    private PotionBag potionBag => this.bagContext.PotionBag;
    private PotionBag playerPotionBag => this.bagContext.PlayerPotionBag;
    private PotionBag targetPotionBag => this.usePlayer ? this.playerPotionBag : this.potionBag;

    void Update()
    {
        StringBuilder sb = new StringBuilder(this.header + "\n");
        foreach (var (potion, count) in this.targetPotionBag.GetAll())
        {
            sb.AppendLine($"- {potion.potionName} x{count}");
        }
        this.text.text = sb.ToString();
    }
}
