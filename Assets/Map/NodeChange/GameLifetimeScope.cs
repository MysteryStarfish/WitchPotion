using VContainer;
using VContainer.Unity;
using MessagePipe;
using UnityEngine;
using System.Collections.Generic;
using WitchPotion.Bag;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private List<Herb> herbs;
    [SerializeField] private List<Potion> potions;
    [SerializeField] private List<PotionFormula> potionFormulas;

    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();

        builder.RegisterInstance(new HerbRepository(this.herbs));
        Debug.Log($"Loaded {this.herbs.Count} herbs");
        builder.RegisterInstance(new PotionRepository(this.potions, this.potionFormulas));
        Debug.Log($"Loaded {this.potions.Count} potions and {this.potionFormulas.Count} formulas");
        builder.Register<BagContext>(Lifetime.Singleton);
    }
}

