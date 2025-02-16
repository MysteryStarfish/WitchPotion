using UnityEngine;
using System.Collections.Generic;

public class PotionRepository
{
    public List<Potion> All { get; }

    public PotionRepository(List<Potion> potions)
    {
        All = potions;
    }
}
