using UnityEngine;
using VContainer;

namespace WitchPotion.Bag
{
    public class BagContext
    {
        public HerbBag HerbBag { get; private set; }

        public BagContext(HerbRepository herbRepository)
        {
            this.HerbBag = new HerbBag(herbRepository);
        }
    }
}

