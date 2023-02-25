using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class WoodBoard : Board
    {
        [SerializeField] private List<BlockData> _Board_Specific_Spawns;
        public override void NextDay()
        {
            if (!_is_Unlocked)
            {
                return;
            }
            base.NextDay();
            foreach (var l_Block in _Board_Specific_Spawns)
            {
                Try_Spawning_Block_On_Board(l_Block.id, this.transform.position);
            }
        }
    }
}
