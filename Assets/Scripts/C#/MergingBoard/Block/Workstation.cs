using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Workstation : Block
    {
        [SerializeField] public int remainingCharges;
        [SerializeField] public int currentProductionDay = 1;
        [SerializeField] public List<int> item_Buffer;
        [SerializeField] public bool hasWorker;
        [SerializeField] public Worker_Save_Data saved_Worker;

        public void init_Block(WorkStationData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remainingCharges = init_Block_Data.charges;
            item_Buffer = new List<int>();
        }

        public void init_Block(WorkStationData init_Block_Data, float p_Travel_Time, WorkStation_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            remainingCharges = block_Save.remaining_Charges;
            currentProductionDay = block_Save.current_Production_Day;
            item_Buffer = block_Save.item_Buffer;
            hasWorker = block_Save.hasWorker;
            saved_Worker = block_Save.savedWorker;
        }

        private void Update()
        {
            if (hasWorker && !saved_Worker.isTired)
            {
                Work();
            }
            if (item_Buffer.Count > 0)
            {
                Board parent_Board = this.GetComponentInParent<Board>();
                if (parent_Board.Try_Spawn_Block_In_Neighborhood(item_Buffer[0], Parent_Node))
                {
                    item_Buffer.RemoveAt(0);
                }
            }
            if (hasWorker && (saved_Worker.isTired || (remainingCharges <= 0)))
            {
                TryRemoveWorker();
            }
            if (remainingCharges <= 0 && !hasWorker && item_Buffer.Count <= 0)
            {
                this.GetComponentInParent<Board>().RemoveBlock(this);
            }
        }

        public new WorkStation_Save_Data SaveBlock()
        {
            WorkStation_Save_Data workStation_Save = new WorkStation_Save_Data(base.SaveBlock(), remainingCharges, currentProductionDay, item_Buffer, hasWorker, saved_Worker);
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

        private void Work()
        {
            if (hasWorker && !saved_Worker.isTired && item_Buffer.Count == 0 && currentProductionDay == ((WorkStationData)block_Data).productionDays && remainingCharges > 0)
            {
                for (int i = 0; i < ((WorkStationData)block_Data).items_Per_Charge; i++)
                {
                    item_Buffer.Add(((WorkStationData)block_Data).possible_Results.GetRandom().id);
                }
                currentProductionDay = 1;
                saved_Worker.isTired = true;
                saved_Worker.isHungry = true;
                if (!((WorkStationData)block_Data).isLimitless)
                {
                    remainingCharges -= 1;
                }
            }
            else if (hasWorker && !saved_Worker.isTired && currentProductionDay < ((WorkStationData)block_Data).productionDays && remainingCharges > 0)
            {
                currentProductionDay += 1;
            }
        }

        public bool TryTakeWorker(Worker worker)
        {
            if (!hasWorker && !worker.isTired && ((WorkStationData)block_Data).allowedJobs.Contains(((WorkerData)(worker.block_Data)).job))
            {
                Board parent_Board = worker.GetComponentInParent<Board>();
                saved_Worker = worker.SaveBlock();
                parent_Board.RemoveBlock(worker);
                hasWorker = true;
                return true;
            }
            return false;
        }

        public void TryRemoveWorker()
        {
            Board parent_Board = this.GetComponentInParent<Board>();
            if (parent_Board.Try_Spawning_Worker_Block_On_Board_With_Save(saved_Worker.base_Block_Save.id, this.transform.position, saved_Worker))
                hasWorker = false;
        }
    }

    [System.Serializable]
    public struct WorkStation_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int remaining_Charges;
        public int current_Production_Day;
        public List<int> item_Buffer;
        public bool hasWorker;
        public Worker_Save_Data savedWorker;

        public WorkStation_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            int p_Remaining_Charges,
            int p_Current_Production_Day,
            List<int> p_Item_Buffer,
            bool p_HasWorker,
            Worker_Save_Data p_SavedWorker)
        {
            base_Block_Save = p_Base_Block_Save;
            remaining_Charges = p_Remaining_Charges;
            current_Production_Day = p_Current_Production_Day;
            item_Buffer = p_Item_Buffer;
            hasWorker = p_HasWorker;
            savedWorker = p_SavedWorker;
        }
    }
}
