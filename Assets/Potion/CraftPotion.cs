using System.Collections.Generic;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class CraftPotion : MonoBehaviour
{
    [Inject]
    private BagContext bagContext;
    [Inject]
    private readonly PotionRepository potionRepository;
    private List<PotionFormula> potionFormulas => this.potionRepository.AllFormulas;

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
