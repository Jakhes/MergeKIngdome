using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Workstation", menuName = "BlockData/Workstation", order = 0)]
    public class WorkStationData : BlockData
    {
        [SerializeField] public WeightedRandomList<BlockData> possible_Results;
        [SerializeField] public bool isLimitless;
        [SerializeField] public int charges;
        [SerializeField] public int needed_Labor;
        [SerializeField] public int items_Per_Charge;
        [SerializeField] public int required_Worker_Tier;
    }
}
