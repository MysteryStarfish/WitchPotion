using System.Linq;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class PotionBagPanel : MonoBehaviour
{
    [SerializeField]
    private bool usePlayer;

    [Inject]
    private BagContext bagContext;
    private PotionBag potionBag => this.bagContext.PotionBag;
    private PotionBag playerPotionBag => this.bagContext.PlayerPotionBag;
    private PotionBag targetPotionBag => this.usePlayer ? this.playerPotionBag : this.potionBag;

    private ItemCell[] itemCells;

    private void Start()
    {
        this.itemCells = GetComponentsInChildren<ItemCell>();
        foreach (var itemCell in this.itemCells)
        {
            itemCell.RemoveItem();
        }
    }

    void Update()
    {
        var itemCellsIter = this.itemCells.AsEnumerable().GetEnumerator();
        foreach (var (potion, count) in this.targetPotionBag.GetAll())
        {
            if (!itemCellsIter.MoveNext())
            {
                break;
            }
            itemCellsIter.Current.SetItem(potion.sprite, count);
        }
    }
}
