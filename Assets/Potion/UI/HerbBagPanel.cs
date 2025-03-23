using System;
using System.Linq;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class HerbBagPanel : MonoBehaviour
{
    [Inject]
    private BagContext bagContext;
    private HerbBag herbBag => this.bagContext.HerbBag;

    private ItemCell[] itemCells;

    public Func<Herb, bool> filter;

    private void Start()
    {
        this.itemCells = GetComponentsInChildren<ItemCell>();
        foreach (var itemCell in this.itemCells)
        {
            itemCell.RemoveItem();
        }
        this.herbBag.SetCount("101", 10);
        this.herbBag.SetCount("102", 10);
    }

    void Update()
    {
        // clear item cells
        foreach (var itemCell in this.itemCells)
        {
            itemCell.RemoveItem();
        }

        var itemCellsIter = this.itemCells.AsEnumerable().GetEnumerator();
        foreach (var (herb, count) in this.herbBag.GetAll())
        {
            if (this.filter != null && !this.filter(herb))
            {
                // Skip if the filter is set and the item does not pass the filter.
                continue;
            }

            if (!itemCellsIter.MoveNext())
            {
                break;
            }
            itemCellsIter.Current.SetItem(herb.sprite, count, $"Herb:{herb.code}");
        }
    }
}
