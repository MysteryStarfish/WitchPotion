using System.Collections.Generic;
using UnityEngine;

namespace WitchPotion.Bag
{
    public class PotionBag : Bag<Potion>
    {
        private PotionRepository potionRepository;
        private Dictionary<string, int> potionCounts;

        public PotionBag(PotionRepository potionRepository)
        {
            this.potionRepository = potionRepository;
            this.potionCounts = new Dictionary<string, int>();
        }

        public Potion Get(string itemCode)
        {
            return potionRepository.All.Find(potion => potion.code == itemCode);
        }

        public IEnumerable<(Potion, int)> GetAll()
        {
            foreach (var potion in potionRepository.All)
            {
                if (potionCounts.ContainsKey(potion.code))
                {
                    yield return (potion, potionCounts[potion.code]);
                }
            }
        }

        public int GetCount(string itemCode)
        {
            return potionCounts.ContainsKey(itemCode) ? potionCounts[itemCode] : 0;
        }

        public void SetCount(string itemCode, int count)
        {
            Debug.Assert(this.potionRepository.All.Exists(potion => potion.code == itemCode));
            Debug.Assert(count >= 0);

            if (count == 0)
            {
                potionCounts.Remove(itemCode);
            }
            else
            {
                potionCounts[itemCode] = count;
            }
        }
    }
}