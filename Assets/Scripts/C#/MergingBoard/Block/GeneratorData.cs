using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Generator", menuName = "BlockData/Generator", order = 0)]
    public class GeneratorData : BlockData
    {
        [SerializeField] public int charge_Time;
        [SerializeField] public int items_Per_Charge;
        [SerializeField] public int max_ItemBuffer;
        [SerializeField] public WeightedRandomList<BlockData> possible_Results;
        [SerializeField] public bool isNeighborGenerator;
    }
}
