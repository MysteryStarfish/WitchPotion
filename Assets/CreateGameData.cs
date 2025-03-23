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
            var originalHerb = AssetDatabase.LoadAssetAtPath<Herb>(path);
            if (originalHerb != null)
            {
                originalHerb.herbName = herb.herbName;
                originalHerb.element = herb.element;
                originalHerb.level = herb.level;
                originalHerb.description = herb.description;
            }
            else
            {
                AssetDatabase.CreateAsset(herb, path);
            }
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

    static void makePotion(Potion potion, string code, string potionName, string element, string description)
    {
        if (potion == null)
        {
            potion = ScriptableObject.CreateInstance<Potion>();
        }

        potion.code = code;
        potion.potionName = potionName;
        potion.element = element;
        potion.description = description;

        string spritePath = "Assets/Sprites/Potion/" + potion.code + ".png";
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogWarning("Sprite not found: " + spritePath);
        }
        potion.sprite = sprite;
    }


    // Helper class to store potion information
    class PotionInfo
    {
        public string Code;
        public string Name;
        public string Element;
        public string Description;
        public (string code, int count)[] Ingredients;

        public PotionInfo(string code, string name, string element, string description, (string code, int count)[] ingredients)
        {
            Code = code;
            Name = name;
            Element = element;
            Description = description;
            Ingredients = ingredients;
        }
    }

    [MenuItem("WitchPotion/Create Potions")]
    static void createPotions()
    {
        List<Potion> potions = new List<Potion>();


        // Helper method to create a potion and its formula
        static void CreatePotionWithFormula(PotionInfo info)
        {
            // Create or update the potion
            string potionPath = "Assets/GameData/Potion/" + info.Code + ".asset";
            var potion = AssetDatabase.LoadAssetAtPath<Potion>(potionPath);
            if (potion == null)
            {
                potion = ScriptableObject.CreateInstance<Potion>();
                AssetDatabase.CreateAsset(potion, potionPath);
            }

            makePotion(potion, info.Code, info.Name, info.Element, info.Description);

            // Create or update the formula
            string formulaPath = "Assets/GameData/PotionFormula/" + info.Code + ".asset";
            var formula = AssetDatabase.LoadAssetAtPath<PotionFormula>(formulaPath);
            if (formula == null)
            {
                formula = ScriptableObject.CreateInstance<PotionFormula>();
                AssetDatabase.CreateAsset(formula, formulaPath);
            }

            formula.potionCode = info.Code;
            formula.herbCodes = new List<string>();
            formula.herbCounts = new List<int>();

            foreach (var (code, count) in info.Ingredients)
            {
                formula.herbCodes.Add(code);
                formula.herbCounts.Add(count);
            }

            EditorUtility.SetDirty(potion);
            EditorUtility.SetDirty(formula);
        }

        // Create list of all potions from the data
        List<PotionInfo> potionInfos = new List<PotionInfo>
        {
            // Explosive potions (爆炸藥水)
            new PotionInfo("1001", "一階爆炸藥水", "融合", "使前方障礙清除", new[] { ("301", 2), ("401", 2) }),
            new PotionInfo("1002", "二階爆炸藥水", "融合", "", new[] { ("302", 2), ("402", 2) }),
            new PotionInfo("1003", "三階爆炸藥水", "融合", "", new[] { ("303", 2), ("403", 2) }),
            new PotionInfo("1004", "四階爆炸藥水", "融合", "", new[] { ("304", 2), ("404", 2) }),
            
            // Wind blade potions (風刃藥水)
            new PotionInfo("1011", "一階風刃藥水", "融合", "", new[] { ("401", 2), ("501", 2), ("101", 1) }),
            new PotionInfo("1012", "二階風刃藥水", "融合", "", new[] { ("402", 2), ("502", 2), ("102", 1) }),
            new PotionInfo("1013", "三階風刃藥水", "融合", "", new[] { ("403", 2), ("503", 2), ("103", 1) }),
            new PotionInfo("1014", "四階風刃藥水", "融合", "", new[] { ("404", 2), ("504", 2), ("104", 1) }),
            
            // Counterbalance potions (抗衡藥水)
            new PotionInfo("1021", "一階抗衡藥水", "融合", "", new[] { ("502", 2), ("301", 2), ("201", 1) }),
            new PotionInfo("1022", "二階抗衡藥水", "融合", "", new[] { ("503", 2), ("302", 2), ("202", 1) }),
            new PotionInfo("1023", "三階抗衡藥水", "融合", "", new[] { ("504", 2), ("303", 2), ("203", 1) }),
            new PotionInfo("1024", "四階抗衡藥水", "融合", "", new[] { ("505", 2), ("304", 2), ("204", 1) }),
            
            // Strength potions (強壯藥水)
            new PotionInfo("1031", "一階強壯藥水", "融合", "強化力量擊碎阻擋的石頭", new[] { ("201", 2), ("202", 2) }),
            new PotionInfo("1032", "二階強壯藥水", "融合", "", new[] { ("202", 2), ("203", 2) }),
            new PotionInfo("1033", "三階強壯藥水", "融合", "", new[] { ("203", 2), ("204", 2) }),
            new PotionInfo("1034", "四階強壯藥水", "融合", "", new[] { ("204", 2), ("205", 2) }),
            
            // Corrosion potions (腐蝕藥水)
            new PotionInfo("1041", "一階腐蝕藥水", "融合", "", new[] { ("501", 2), ("203", 2) }),
            new PotionInfo("1042", "二階腐蝕藥水", "融合", "", new[] { ("502", 2), ("204", 2) }),
            new PotionInfo("1043", "三階腐蝕藥水", "融合", "", new[] { ("503", 2), ("205", 2) }),
            new PotionInfo("1044", "四階腐蝕藥水", "融合", "", new[] { ("504", 2), ("206", 2) }),
            
            // Fire potions (燃火藥水)
            new PotionInfo("1051", "一階燃火藥水", "融合", "", new[] { ("302", 2), ("403", 1) }),
            new PotionInfo("1052", "二階燃火藥水", "融合", "", new[] { ("303", 2), ("404", 1) }),
            new PotionInfo("1053", "三階燃火藥水", "融合", "", new[] { ("304", 2), ("405", 1) }),
            new PotionInfo("1054", "四階燃火藥水", "融合", "", new[] { ("305", 2), ("406", 1) }),
            
            // Spring potions (湧泉藥水)
            new PotionInfo("1061", "一階湧泉藥水", "融合", "", new[] { ("501", 3), ("201", 1), ("503", 1) }),
            new PotionInfo("1062", "二階湧泉藥水", "融合", "", new[] { ("502", 2), ("202", 1), ("504", 1) }),
            new PotionInfo("1063", "三階湧泉藥水", "融合", "", new[] { ("503", 2), ("203", 1), ("505", 1) }),
            new PotionInfo("1064", "四階湧泉藥水", "融合", "", new[] { ("504", 2), ("204", 1), ("506", 1) }),
            
            // X-ray potions (透視藥水)
            new PotionInfo("1071", "一階透視藥水", "融合", "可以看到該房間隱藏區域的入口", new[] { ("503", 3), ("101", 2) }),
            new PotionInfo("1072", "二階透視藥水", "融合", "", new[] { ("504", 3), ("102", 2) }),
            new PotionInfo("1073", "三階透視藥水", "融合", "", new[] { ("505", 3), ("103", 2) }),
            new PotionInfo("1074", "四階透視藥水", "融合", "", new[] { ("506", 3), ("104", 2) }),
            
            // Curse breaking potions (破咒藥水)
            new PotionInfo("1081", "一階破咒藥水", "融合", "", new[] { ("402", 3), ("503", 3), ("102", 1) }),
            new PotionInfo("1082", "二階破咒藥水", "融合", "", new[] { ("403", 3), ("504", 3), ("103", 1) }),
            new PotionInfo("1083", "三階破咒藥水", "融合", "", new[] { ("404", 3), ("505", 3), ("104", 1) }),
            new PotionInfo("1084", "四階破咒藥水", "融合", "", new[] { ("405", 3), ("506", 3), ("105", 1) }),
            
            // Agility potions (靈動藥水)
            new PotionInfo("1091", "一階靈動藥水", "融合", "使用多跑一格不消耗步數", new[] { ("401", 2), ("403", 2) }),
            new PotionInfo("1092", "二階靈動藥水", "融合", "", new[] { ("402", 2), ("404", 2) }),
            new PotionInfo("1093", "三階靈動藥水", "融合", "", new[] { ("403", 2), ("405", 2) }),
            new PotionInfo("1094", "四階靈動藥水", "融合", "", new[] { ("404", 2), ("406", 2) }),
            
            // Interpretation potions (解讀藥水)
            new PotionInfo("1101", "一階解讀藥水", "融合", "和石頭溝通", new[] { ("102", 3), ("303", 2) }),
            new PotionInfo("1102", "二階解讀藥水", "融合", "", new[] { ("103", 3), ("304", 2) }),
            new PotionInfo("1103", "三階解讀藥水", "融合", "", new[] { ("104", 3), ("305", 2) }),
            new PotionInfo("1104", "四階解讀藥水", "融合", "", new[] { ("105", 3), ("306", 2) }),
            
            // Special potions (特殊藥水)
            new PotionInfo("1901", "幸運藥水", "特殊", "拿到材料翻倍", new[] { ("505", 3), ("104", 2), ("107", 2) }),
            new PotionInfo("1902", "超幸運藥水", "特殊", "拿到材料三倍", new[] { ("505", 5), ("104", 5), ("107", 3), ("108", 2) }),
            new PotionInfo("1904", "躍域藥水", "特殊", "跳過一格不算步數", new[] { ("206", 4), ("406", 3), ("106", 2), ("108", 1) }),
            new PotionInfo("1905", "暮路藥水", "特殊", "往西邊移動一格", new[] { ("104", 5), ("305", 2), ("406", 2), ("105", 2) }),
            new PotionInfo("1906", "黎徑藥水", "特殊", "往東邊移動一格", new[] { ("104", 5), ("305", 2), ("306", 2), ("105", 2) }),
            new PotionInfo("1907", "波途藥水", "特殊", "移動到最近一個水屬性藥草的區域", new[] { ("304", 3), ("103", 5), ("105", 2), ("106", 1) }),
            new PotionInfo("1908", "焰途藥水", "特殊", "移動到最近一個火屬性藥草的區域", new[] { ("404", 3), ("103", 5), ("105", 2), ("106", 1) }),
            new PotionInfo("1909", "颯徑藥水", "特殊", "移動到最近一個風屬性藥草的區域", new[] { ("204", 3), ("103", 5), ("105", 2), ("106", 1) }),
            new PotionInfo("1910", "堰徑藥水", "特殊", "移動到最近一個物理性藥草的區域", new[] { ("504", 3), ("103", 5), ("105", 2), ("106", 1) }),
        };

        // Create all the potions and formulas
        foreach (var info in potionInfos)
        {
            CreatePotionWithFormula(info);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Created/Updated {potionInfos.Count} potions and formulas");
    }
}
