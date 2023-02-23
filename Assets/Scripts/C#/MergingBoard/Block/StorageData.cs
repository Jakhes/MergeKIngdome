using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Storage", menuName = "BlockData/Storage", order = 0)]
    public class StorageData : BlockData
    {
        [SerializeField] public int max_Storage;
        [SerializeField] public List<BlockType> allowed_Types;
    }
}
