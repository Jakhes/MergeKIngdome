using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Worker", menuName = "BlockData/Worker", order = 0)]
    public class WorkerData : BlockData
    {
        [SerializeField] public int maxHP;
        [SerializeField] public int attackDamage;
        [SerializeField] public int defense;
        [SerializeField] public int foodConsumption;
        [SerializeField] public Job job;
    }

    public enum Job
    {
        None,
        Woodcutter,
        Miner,
        Farmer,
        Solder
    }
}
