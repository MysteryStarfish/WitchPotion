using UnityEngine;
using System.Collections.Generic;

public class PotionRepository
{
    public List<Potion> All { get; }
    public List<PotionFormula> AllFormulas { get; }

    public PotionRepository(List<Potion> potions, List<PotionFormula> potionFormulas)
    {
        All = potions;
        AllFormulas = potionFormulas;
    }
}
