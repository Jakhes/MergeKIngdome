using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Unlockable", menuName = "BlockData/Unlockable", order = 0)]
    public class UnlockableData : BlockData
    {
        [SerializeField] public int price;
        [SerializeField] public BlockData underlyingBlock;
    }
}
