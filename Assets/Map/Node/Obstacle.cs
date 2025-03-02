using System.Collections.Generic;
using WitchPotion.Bag;

namespace Map
{
    public class Obstacle
    {
        public enum ObstacleType
        {
            石頭堆,
            刺藤團,
            牧草捆,
            黏液網,
            蛛絲地,
            瘴氣區,
            凍土台,
            蒸氣口,
            炎岩區,
            迷幻院
        }

        private Dictionary<ObstacleType, List<string>> _unlockPotionId = new Dictionary<ObstacleType, List<string>>()
        {
            { ObstacleType.石頭堆, new List<string>() { "爆炸藥水", "1001", "1002", "1003", "1004", "1011", "1012", "1013", "1031", "1032", "1041" } },
            { ObstacleType.刺藤團, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.牧草捆, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.黏液網, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.蛛絲地, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.瘴氣區, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.凍土台, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.蒸氣口, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.炎岩區, new List<string>() { "1051", "1052", "1053", "1054", "1031", "1032", "1033", "1011", "1012", "1041" } },
            { ObstacleType.迷幻院, new List<string>() { "爆炸藥水" } },
        };
        public ObstacleType Type { get; private set; }
        private int _level;
        public Obstacle(int type, int level)
        {
            Type = (ObstacleType)type;
            _level = level;
        }
        public bool IsCorrectPotion(Potion potion)
        {
            if (_unlockPotionId[Type].Contains(potion.potionName)) return true;
            return false;
        }
    }
}