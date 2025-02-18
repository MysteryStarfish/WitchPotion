using System.Collections.Generic;
using UnityEngine;

namespace WitchPotion.Bag
{
    public class HerbBag : Bag<Herb>
    {
        private HerbRepository herbRepository;
        private Dictionary<string, int> herbCounts;

        public HerbBag(HerbRepository herbRepository)
        {
            this.herbRepository = herbRepository;
            this.herbCounts = new Dictionary<string, int>();
        }

        public Herb Get(string itemCode)
        {
            return herbRepository.All.Find(herb => herb.code == itemCode);
        }

        public IEnumerable<(Herb, int)> GetAll()
        {
            foreach (var herb in herbRepository.All)
            {
                if (herbCounts.ContainsKey(herb.code))
                {
                    yield return (herb, herbCounts[herb.code]);
                }
            }
        }

        public int GetCount(string itemCode)
        {
            return herbCounts.ContainsKey(itemCode) ? herbCounts[itemCode] : 0;
        }

        public void SetCount(string itemCode, int count)
        {
            if (count == 0)
            {
                herbCounts.Remove(itemCode);
            }
            else
            {
                herbCounts[itemCode] = count;
            }
        }
    }
}

