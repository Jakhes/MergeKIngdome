using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Farm", menuName = "BlockData/Farm", order = 0)]
    public class FarmData : BlockData
    {
        public int max_Slots;
        public FarmType farmType;
        public bool needs_SecondaryResource;
        public BlockData secondaryResource;
        public bool needs_Labor;
        public int needed_Labor;
        public int needed_Days;
    }
}
