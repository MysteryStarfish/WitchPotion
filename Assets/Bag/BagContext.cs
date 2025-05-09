using UnityEngine;
using VContainer;

namespace WitchPotion.Bag
{
    public class BagContext
    {
        public HerbBag HerbBag { get; private set; }
        public PotionBag PotionBag { get; private set; }
        public HerbBag PlayerHerbBag { get; private set; }
        public PotionBag PlayerPotionBag { get; private set; }

        public BagContext(
            HerbRepository herbRepository,
            PotionRepository potionRepository
        )
        {
            this.HerbBag = new HerbBag(herbRepository);
            this.PotionBag = new PotionBag(potionRepository);
            this.PlayerHerbBag = new HerbBag(herbRepository);
            this.PlayerPotionBag = new PotionBag(potionRepository);
        }
    }
}

