using System.Text;
using TMPro;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class PotionBagPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [Inject]
    private BagContext bagContext;

    private PotionBag potionBag => this.bagContext.PotionBag;

    void Start()
    {

    }

    void Update()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Potion Bag");
        foreach (var (potion, count) in this.potionBag.GetAll())
        {
            sb.AppendLine($"- {potion.potionName} x{count}");
        }
        this.text.text = sb.ToString();
    }
}
