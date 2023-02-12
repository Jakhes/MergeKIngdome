using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Shop", menuName = "BlockData/Shop", order = 0)]
    public class ShopData : BlockData
    {
        [SerializeField] public List<ShopEntry> shopEntries;
    }
}
