using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class HerbBagPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [Inject]
    private BagContext bagContext;

    private HerbBag herbBag => this.bagContext.HerbBag;

    void Start()
    {
        this.herbBag.SetCount("301", 10);
        this.herbBag.SetCount("401", 10);
    }

    void Update()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Herb Bag");
        foreach (var (herb, count) in this.herbBag.GetAll())
        {
            sb.AppendLine($"- {herb.herbName} ({herb.code}) x{count}");
        }
        this.text.text = sb.ToString();
    }
}
