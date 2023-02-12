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
}
