using UnityEngine;

namespace WitchPotion.Bag
{
    public interface BagDisplayItem
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }
    }
}