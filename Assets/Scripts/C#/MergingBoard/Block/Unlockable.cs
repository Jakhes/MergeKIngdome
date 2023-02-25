using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Unlockable : Block
    {
        [SerializeField] private SpriteRenderer underlying_Block_Sprite;

        public void init_Block(UnlockableData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            underlying_Block_Sprite.sprite = init_Block_Data.underlyingBlock.sprite;
        }

        public void init_Block(UnlockableData init_Block_Data, float p_Travel_Time, Unlockable_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            underlying_Block_Sprite.sprite = init_Block_Data.underlyingBlock.sprite;
        }

        public new Unlockable_Save_Data SaveBlock()
        {
            Unlockable_Save_Data unlockable_Save = new Unlockable_Save_Data(base.SaveBlock());
            return unlockable_Save;
        }
    }

    [System.Serializable]
    public struct Unlockable_Save_Data
    {
        public Block_Save_Data base_Block_Save;

        public Unlockable_Save_Data(Block_Save_Data p_Base_Block_Save)
        {
            base_Block_Save = p_Base_Block_Save;
        }
    }
}
