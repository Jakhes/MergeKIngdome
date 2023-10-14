using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Worker", menuName = "BlockData/Worker", order = 0)]
    public class WorkerData : BlockData
    {
        [SerializeField] public int max_Labor;
        [SerializeField] public int tier;
        [SerializeField] public UpgradeableData workerUpgrade;
    }

    // not getting used anymore changed from different workers with Jobs to just 3 tiered Robot Workers
    public enum Job
    {
        None,
        Woodcutter,
        Miner,
        Farmer,
        Solder
    }
}
