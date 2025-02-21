using UnityEngine;
using WitchPotion.Bag;

[CreateAssetMenu(fileName = "Potion", menuName = "WitchPotion/Potion")]
public class Potion : ScriptableObject, BagDisplayItem
{
    public string potionName;
    [Tooltip("藥性")]
    public string element;
    public string description;
    public Sprite sprite;

    public Sprite Sprite => sprite;
}
