using System;
using System.Collections;
using System.Collections.Generic;
using EvolvingCode.IngameMessages;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Workstation : Block
    {
        [SerializeField] public int remainingCharges;
        [SerializeField] public int current_Needed_Labor;
        [SerializeField] public List<int> item_Buffer;

        public void init_Block(WorkStationData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remainingCharges = init_Block_Data.charges;
            item_Buffer = new List<int>();
            current_Needed_Labor = init_Block_Data.needed_Labor;
        }

        public void init_Block(WorkStationData init_Block_Data, float p_Travel_Time, WorkStation_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remainingCharges = block_Save.remaining_Charges;
            current_Needed_Labor = block_Save.current_Needed_Labor;
            item_Buffer = block_Save.item_Buffer;
        }

        private void Update()
        {

            if (item_Buffer.Count > 0)
            {
                TryEmptyStorage();
            }

            if (remainingCharges <= 0 && item_Buffer.Count <= 0)
            {
                InfoMessageManager l_InfoMessageManager = this.GetComponentInParent<Board>().InfoMessageManager;
                l_InfoMessageManager.NoChargesLeft(transform.position);
                this.GetComponentInParent<Board>().RemoveBlock(this);
            }

            GenerateIfReady();
        }

        public new WorkStation_Save_Data SaveBlock()
        {
            WorkStation_Save_Data workStation_Save = new WorkStation_Save_Data(base.SaveBlock(), remainingCharges, current_Needed_Labor, item_Buffer);
            return workStation_Save;
        }

        public override void DoubleClickAction()
        {
            if (item_Buffer.Count > 0)
            {
                TryEmptyStorage();
            }
        }

        public void TryEmptyStorage()
        {
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

        private void GenerateIfReady()
        {
            if (item_Buffer.Count <= 0 && current_Needed_Labor <= 0 && remainingCharges > 0)
            {
                SuccessMessageManager l_SuccessMessageManager = this.GetComponentInParent<Board>().SuccessMessageManager;
                l_SuccessMessageManager.Generating(transform.position);

                for (int i = 0; i < ((WorkStationData)block_Data).items_Per_Charge; i++)
                {
                    item_Buffer.Add(((WorkStationData)block_Data).possible_Results.GetRandom().id);
                }
                current_Needed_Labor = ((WorkStationData)block_Data).needed_Labor;
                if (!((WorkStationData)block_Data).isLimitless)
                {
                    remainingCharges -= 1;
                }
            }
        }

        public bool TryToLabor(Worker worker)
        {
            if (((WorkStationData)block_Data).allowedJobs.Contains(((WorkerData)(worker.block_Data)).job))
            {
                current_Needed_Labor = worker.UseLabor(current_Needed_Labor);
                if (current_Needed_Labor > 0)
                {
                    InfoMessageManager l_InfoMessageManager = this.GetComponentInParent<Board>().InfoMessageManager;
                    l_InfoMessageManager.LaborUsed(((WorkStationData)block_Data).needed_Labor - current_Needed_Labor, ((WorkStationData)block_Data).needed_Labor, transform.position);
                }
            }
            return false;
        }
    }

    [System.Serializable]
    public struct WorkStation_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int remaining_Charges;
        public int current_Needed_Labor;
        public List<int> item_Buffer;

        public WorkStation_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            int p_Remaining_Charges,
            int p_Current_Needed_Labor,
            List<int> p_Item_Buffer)
        {
            base_Block_Save = p_Base_Block_Save;
            remaining_Charges = p_Remaining_Charges;
            current_Needed_Labor = p_Current_Needed_Labor;
            item_Buffer = p_Item_Buffer;
        }
    }
}
