using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionFormula", menuName = "WitchPotion/PotionFormula")]
public class PotionFormula : ScriptableObject
{
    public List<string> herbCodes;
    public List<int> herbCounts;

    public string potionName;
}
