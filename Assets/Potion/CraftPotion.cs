using System.Collections.Generic;
using UnityEngine;

public class CraftPotion : MonoBehaviour
{
    // TODO: inject 
    [SerializeField]
    private List<PotionFormula> potionFormulas;

    public Potion Craft(Dictionary<string, int> herbs)
    {
        foreach (var potionFormula in potionFormulas)
        {
            if (this.isMatch(potionFormula, herbs))
            {
                return this.findPotion(potionFormula.potionName);
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

    // TODO: impl
    private Potion findPotion(string potionName)
    {
        var tmp = new Potion();
        tmp.name = potionName;
        return tmp;
    }
}
