using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "WitchPotion/Potion")]
public class Potion : ScriptableObject
{
    public string potionName;
    [Tooltip("藥性")]
    public string element;
    public string description;
    public Sprite icon;
}
