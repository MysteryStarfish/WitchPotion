using UnityEngine;
using WitchPotion.Bag;

// 藥草
[CreateAssetMenu(fileName = "Herb", menuName = "WitchPotion/Herb")]
public class Herb : ScriptableObject, BagDisplayItem
{
    [Tooltip("代號")]
    public string code;
    [Tooltip("藥草名稱")]
    public string herbName;
    [Tooltip("藥性")]
    public string element;
    [Tooltip("等級")]
    public int level;
    [Tooltip("描述")]
    public string description;
    [Tooltip("圖片，顯示於背包等 UI 中")]
    public Sprite sprite;

    public Sprite Sprite => sprite;
}
