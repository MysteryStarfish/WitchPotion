using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class HerbBagPanel : MonoBehaviour
{
    [Inject]
    private BagContext bagContext;
    private HerbBag herbBag => this.bagContext.HerbBag;

    private ItemCell[] itemCells;

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
        var itemCellsIter = this.itemCells.AsEnumerable().GetEnumerator();
        foreach (var (herb, count) in this.herbBag.GetAll())
        {
            if (!itemCellsIter.MoveNext())
            {
                break;
            }
            itemCellsIter.Current.SetItem(herb.sprite, count);
        }
    }
}
