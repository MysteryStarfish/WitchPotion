using UnityEngine;

// 藥草
[CreateAssetMenu(fileName = "Herb", menuName = "WitchPotion/Herb")]
public class Herb : ScriptableObject
{
    [Tooltip("代號")]
    public string code;
    [Tooltip("藥草名稱")]
    public string herbName;
    [Tooltip("藥性")]
    public string element;
    [Tooltip("等級")]
    public int level;
}
