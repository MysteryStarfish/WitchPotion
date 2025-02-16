using System.Collections.Generic;
using UnityEngine;

public class HerbRepository
{
    public List<Herb> All { get; }

    public HerbRepository(List<Herb> herbs)
    {
        All = herbs;
    }
}
