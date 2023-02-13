using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Generator : Block
    {
        [SerializeField] public int remaining_Time;
        [SerializeField] public bool isCharging;
        [SerializeField] public List<int> item_Buffer;



        public void init_Block(GeneratorData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remaining_Time = 0;
            isCharging = false;
            item_Buffer = new List<int>();
        }

        public void init_Block(GeneratorData init_Block_Data, float p_Travel_Time, Generator_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remaining_Time = block_Save.remaining_Time;
            isCharging = block_Save.isCharging;
            item_Buffer = block_Save.item_Buffer;

        }

        private void Start_Charging()
        {

            if (item_Buffer.Count <= ((GeneratorData)block_Data).max_ItemBuffer - ((GeneratorData)block_Data).items_Per_Charge && !isCharging)
            {
                isCharging = true;
                remaining_Time = ((GeneratorData)block_Data).charge_Time;
            }
        }

        private void AddCharge()
        {
            for (int i = 0; i < ((GeneratorData)block_Data).items_Per_Charge; i++)
            {
                item_Buffer.Add(((GeneratorData)block_Data).possible_Results.GetRandom().id);
            }
            isCharging = false;
            Start_Charging();
        }

        public void Advance_Charge_Time()
        {
            if (isCharging)
            {
                remaining_Time -= 1;
                if (remaining_Time <= 0)
                {
                    AddCharge();
                }
            }
        }

        private void Update()
        {
            if (!isCharging)
            {
                Start_Charging();
            }
            if (item_Buffer.Count > 0 && ((GeneratorData)block_Data).isNeighborGenerator)
            {
                Board parent_Board = this.GetComponentInParent<Board>();
                if (parent_Board.Try_Spawn_Block_In_Neighborhood(item_Buffer[0], Parent_Node))
                {
                    item_Buffer.RemoveAt(0);
                }
            }
        }

        public new Generator_Save_Data SaveBlock()
        {
            Generator_Save_Data generator_Save = new Generator_Save_Data(base.SaveBlock(), isCharging, remaining_Time, item_Buffer);
            return generator_Save;
        }

        public override void DoubleClickAction()
        {
            if (item_Buffer.Count > 0 && !((GeneratorData)block_Data).isNeighborGenerator)
            {
                Board parent_Board = this.GetComponentInParent<Board>();
                if (parent_Board.Try_Spawning_Block_On_Board(item_Buffer[0], Parent_Node.transform.position))
                {
                    item_Buffer.RemoveAt(0);
                }
            }
        }

        public void TryEmptyStorage()
        {
            if (((GeneratorData)block_Data).isNeighborGenerator)
            {
                return;
            }
            Board parent_Board = this.GetComponentInParent<Board>();
            List<int> rest_Buffer = new List<int>();
            foreach (int id in item_Buffer)
            {
                if (!parent_Board.Try_Spawning_Block_On_Board(id, Parent_Node.transform.position))
                {
                    rest_Buffer.Add(id);
                }
            }
            item_Buffer = rest_Buffer;
        }
    }

    [System.Serializable]
    public struct Generator_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public bool isCharging;
        public int remaining_Time;
        public List<int> item_Buffer;

        public Generator_Save_Data(Block_Save_Data p_Base_Block_Save, bool p_IsCharging, int p_Remaining_Time, List<int> p_Item_Buffer)
        {
            base_Block_Save = p_Base_Block_Save;
            isCharging = p_IsCharging;
            remaining_Time = p_Remaining_Time;
            item_Buffer = p_Item_Buffer;
        }
    }
}
