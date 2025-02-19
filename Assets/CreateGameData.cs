using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;

public class CreateGameData
{
    [MenuItem("WitchPotion/Create Herbs")]
    static void createHerbs()
    {
        List<Herb> herbs = new List<Herb>();

        herbs.Add(CreateHerb("101", "未知蘑菇", "魔法", "層瓣的扁平類蕈菇，層瓣的外觀與顏色每次生長時與氣候溫度有關，因此都有所不同故而得名"));
        herbs.Add(CreateHerb("102", "夢芝", "魔法", "顏色較為夢幻感(藍粉色)的靈芝(?)"));
        herbs.Add(CreateHerb("103", "實食果", "魔法", "橡實"));
        herbs.Add(CreateHerb("104", "偽羽花", "魔法", "芒花"));
        herbs.Add(CreateHerb("105", "幻象蘑菇", "魔法", "外觀上有許多類似眼睛的斑點，容易讓採摘的人誤會有數十雙眼睛盯著自己，進而讓人產生香菇有生命的混亂"));
        herbs.Add(CreateHerb("106", "蛇尾枝", "魔法", "比較長的鼠尾草"));
        herbs.Add(CreateHerb("107", "珍奇星果", "魔法", "星星狀、內容像星空"));
        herbs.Add(CreateHerb("108", "月華結晶", "魔法", "樹液流出後因接觸到空氣而得到的剔透晶體"));
        herbs.Add(CreateHerb("201", "強壯蘑菇", "物理", "看似普普通通的蘑菇，但表面顏色在光影變化下會有類似雷射包包的光影，在捲邊上也可以看見"));
        herbs.Add(CreateHerb("202", "僵硬果", "物理", ""));
        herbs.Add(CreateHerb("203", "酸澀果", "物理", ""));
        herbs.Add(CreateHerb("204", "菌花", "物理", ""));
        herbs.Add(CreateHerb("205", "刺棘果", "物理", "表面有微微禿起的果子"));
        herbs.Add(CreateHerb("206", "薊芒草", "物理", "洋薊草的更酷版本(?)"));
        herbs.Add(CreateHerb("301", "火靈花", "火", "樹枝上小小的花，樹的大小大約是盆景大小，花蕊會散出花粉類似火光光點"));
        herbs.Add(CreateHerb("302", "楠薔草", "火", ""));
        herbs.Add(CreateHerb("303", "微光果", "火", "淡淡散發光點的圓潤果實，表面似乎可以透光，光點其實是吸收養分的種子，外觀有點像桃子，葉片的部分有點像楓葉，成熟時會變得橘紅"));
        herbs.Add(CreateHerb("304", "血寶實", "火", "暗血色的果實，在陽光下晶瑩剔透像寶石一樣"));
        herbs.Add(CreateHerb("305", "漬油蘑菇", "火", "像是容器般盛裝晶透的油，外觀類似酒杯"));
        herbs.Add(CreateHerb("306", "熔蝕果", "火", "類似流動的熔漿，類似星空橘紅或是惡魔果實"));
        herbs.Add(CreateHerb("401", "風之花", "風", ""));
        herbs.Add(CreateHerb("402", "蝶迷香", "風", "迷迭香"));
        herbs.Add(CreateHerb("403", "風台草", "風", "棕葉狗尾草"));
        herbs.Add(CreateHerb("404", "殘語果", "風", "類似風鈴的果串"));
        herbs.Add(CreateHerb("405", "臭味草", "風", ""));
        herbs.Add(CreateHerb("406", "迷霧蘑菇", "風", "乾癟外還裹有一層類似棉花糖的迷霧感，讓人誤會實際大小"));
        herbs.Add(CreateHerb("501", "葉液", "水", ""));
        herbs.Add(CreateHerb("502", "潤甜果", "水", "通透水潤感的果實，大小比草莓大些"));
        herbs.Add(CreateHerb("503", "水鏡草", "水", ""));
        herbs.Add(CreateHerb("504", "沼爪花", "水", ""));
        herbs.Add(CreateHerb("505", "三鹽漿", "水", "酢漿草外觀的果實，愛心呈現飽滿狀態並且有氣泡感，類似多肉植物的飽滿葉子"));
        herbs.Add(CreateHerb("506", "渦流草", "水", "圓形的葉片，上面會有漩渦的流向感"));

        foreach (Herb herb in herbs)
        {
            string path = "Assets/GameData/Herb/" + herb.code + ".asset";
            AssetDatabase.CreateAsset(herb, path);
        }
    }

    static Herb CreateHerb(string code, string herbName, string element, string description)
    {
        Herb herb = ScriptableObject.CreateInstance<Herb>();
        herb.code = code;
        herb.herbName = herbName;
        herb.element = element;
        herb.level = int.Parse(code.Substring(code.Length - 1));
        herb.description = description;
        return herb;
    }

    [MenuItem("WitchPotion/Assign Herb Sprites")]
    static void assignSprites()
    {
        foreach (string guid in AssetDatabase.FindAssets("t:Herb", new string[] { "Assets/GameData/Herb" }))
        {
            string herbPath = AssetDatabase.GUIDToAssetPath(guid);
            Herb herb = AssetDatabase.LoadAssetAtPath<Herb>(herbPath);
            string path = "Assets/Sprites/Herbs/" + herb.code + ".png";
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null)
            {
                Debug.LogWarning("Sprite not found: " + path);
                continue;
            }
            herb.sprite = sprite;
            EditorUtility.SetDirty(herb);
        }
    }
}
