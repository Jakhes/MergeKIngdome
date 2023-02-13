using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Food : Block
    {
        [SerializeField] public int chargesLeft;


        public void init_Block(FoodData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            chargesLeft = init_Block_Data.maxCharges;
        }

        public void init_Block(FoodData init_Block_Data, float p_Travel_Time, Food_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            chargesLeft = block_Save.chargesLeft;
        }

        public new Food_Save_Data SaveBlock()
        {
            Food_Save_Data workStation_Save = new Food_Save_Data(base.SaveBlock(), chargesLeft);
            return workStation_Save;
        }
    }

    [System.Serializable]
    public struct Food_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int chargesLeft;

        public Food_Save_Data(Block_Save_Data p_Base_Block_Save, int p_ChargesLeft)
        {
            base_Block_Save = p_Base_Block_Save;
            chargesLeft = p_ChargesLeft;
        }
    }
}
