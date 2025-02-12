using System;
using System.Collections.Generic;
using UnityEngine;

public class HerbTransformer : MonoBehaviour
{
    // TODO: inject repository
    private Dictionary<string, int> herbRepository = new Dictionary<string, int>()
    {
        { "301", 10 },
        { "401", 10 },
    };

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
        int currentCount = herbRepository.GetValueOrDefault(consumedCode);
        int toConsume = count * 9;
        if (currentCount < toConsume)
        {
            throw new ArgumentException("Not enough herb to upcast");
        }

        herbRepository[consumedCode] = currentCount - toConsume;
        herbRepository[targetCode] = herbRepository.GetValueOrDefault(targetCode) + count * 2;
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
        int currentCount = herbRepository.GetValueOrDefault(consumedCode);
        int toConsume = count * 5;
        if (currentCount < toConsume)
        {
            throw new ArgumentException("Not enough herb to downcast");
        }

        herbRepository[consumedCode] = currentCount - toConsume;
        herbRepository[targetCode] = herbRepository.GetValueOrDefault(targetCode) + count * 2;
    }
}
