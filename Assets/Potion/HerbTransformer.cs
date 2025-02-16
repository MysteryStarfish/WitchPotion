using System;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;

public class HerbTransformer : MonoBehaviour
{
    [Inject]
    private BagContext bagContext;

    private HerbBag herbBag => this.bagContext.HerbBag;

    // ratio 9 => 2
    // note: count 表示要轉換幾份，i.e. 傳入 1 會轉出 2 個藥材
    public void upcast(string targetCode, int count)
    {
        // TODO: check if targetCode is valid

        int level = targetCode[0] - '0';
        if (level >= 5)
        {
            throw new ArgumentException("Herb level must be less than 5");
        }

        string consumedCode = (level - 1) + targetCode.Substring(1);
        int currentCount = this.herbBag.GetCount(consumedCode);
        int toConsume = count * 9;
        if (currentCount < toConsume)
        {
            throw new ArgumentException("Not enough herb to upcast");
        }

        this.herbBag.SetCount(consumedCode, currentCount - toConsume);
        this.herbBag.SetCount(targetCode, this.herbBag.GetCount(targetCode) + count * 2);
    }

    // ratio 5 => 2
    public void downcast(string targetCode, int count)
    {
        // TODO: check if targetCode is valid

        int level = targetCode[0] - '0';
        if (level <= 0)
        {
            throw new ArgumentException("Herb level must >= 0");
        }

        string consumedCode = (level + 1) + targetCode.Substring(1);
        int currentCount = this.herbBag.GetCount(consumedCode);
        int toConsume = count * 5;
        if (currentCount < toConsume)
        {
            throw new ArgumentException("Not enough herb to downcast");
        }

        this.herbBag.SetCount(consumedCode, currentCount - toConsume);
        this.herbBag.SetCount(targetCode, this.herbBag.GetCount(targetCode) + count * 2);
    }
}
