using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "House", menuName = "BlockData/House", order = 0)]
    public class HouseData : BlockData
    {
        [SerializeField] public int roomLimit;
        [SerializeField] public int foodStorageLimit;
    }
}
