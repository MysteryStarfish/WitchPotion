using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using WitchPotion.Bag;

public class Crucible : MonoBehaviour, OnHerbDroppedHandler
{
    [SerializeField]
    private CrucibleContent crucibleContentPrefab;

    [Inject]
    private BagContext bagContext;
    [Inject]
    private readonly PotionRepository potionRepository;
    [Inject]
    private readonly HerbRepository herbRepository;
    private List<PotionFormula> potionFormulas => this.potionRepository.AllFormulas;

    private Dictionary<string, int> herbs = new Dictionary<string, int>();

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            var crucibleContent = Instantiate(this.crucibleContentPrefab, transform.parent);
            foreach (var (code, count) in this.herbs)
            {
                var herb = this.herbRepository.All.Find(herb => herb.code == code);
                crucibleContent.AddHerb(herb.sprite, count);
            }
            crucibleContent.OnCraftButtonClicked.AddListener(() =>
            {
                var potion = this.Craft(herbs);
                if (potion != null)
                {
                    Debug.Log($"Potion {potion.name} crafted");
                }
                else
                {
                    Debug.Log("Craft failed");
                }
                Destroy(crucibleContent.gameObject);
            });
        });
    }

    public bool OnHerbDropped(string herbCode)
    {
        var splits = herbCode.Split(':');
        Debug.Assert(splits.Length == 2 && splits[0] == "Herb", $"Invalid herb code: {herbCode}");
        var actualHerbCode = splits[1];
        var herb = this.herbRepository.All.Find(herb => herb.code == actualHerbCode);
        if (herb == null)
        {
            Debug.Log($"Herb {actualHerbCode} not found");
            return false;
        }

        if (herbs.ContainsKey(herb.code))
        {
            herbs[herb.code]++;
        }
        else
        {
            herbs.Add(herb.code, 1);
        }

        this.bagContext.HerbBag.SetCount(
            actualHerbCode,
            this.bagContext.HerbBag.GetCount(actualHerbCode) - 1
        );
        Debug.Log($"Herb {herb.name} dropped");
        return true;
    }

    public Potion Craft(Dictionary<string, int> herbs)
    {
        foreach (var potionFormula in potionFormulas)
        {
            if (this.isMatch(potionFormula, herbs))
            {
                var potion = this.findPotion(potionFormula.potionCode);
                Debug.Assert(potion != null, $"Potion {potionFormula.potionCode} not found");
                this.bagContext.PotionBag.SetCount(potion.name, this.bagContext.PotionBag.GetCount(potion.name) + 1);
                return potion;
            }
        }

        // return herbs to bag
        foreach (var (code, count) in herbs)
        {
            this.bagContext.HerbBag.SetCount(code, this.bagContext.HerbBag.GetCount(code) + count);
        }
        return null;
    }

    private bool isMatch(PotionFormula potionFormula, Dictionary<string, int> herbs)
    {
        if (potionFormula.herbCodes.Count != herbs.Count)
        {
            return false;
        }

        for (int i = 0; i < potionFormula.herbCodes.Count; i++)
        {
            var herbCode = potionFormula.herbCodes[i];
            var herbCount = potionFormula.herbCounts[i];

            if (!herbs.ContainsKey(herbCode))
            {
                return false;
            }

            if (herbs[herbCode] != herbCount)
            {
                return false;
            }
        }

        return true;
    }

    private Potion findPotion(string code)
    {
        return this.potionRepository.All.Find(potion => potion.code == code);
    }
}
