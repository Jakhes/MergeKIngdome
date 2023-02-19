using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class House : Block
    {
        [SerializeField] public List<Worker_Save_Data> inhabitants;
        [SerializeField] public bool areRoomsEmpty;


        public void init_Block(HouseData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = new List<Worker_Save_Data>();
            areRoomsEmpty = true;
        }

        public void init_Block(HouseData init_Block_Data, float p_Travel_Time, House_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = block_Save.inhabitants;
            areRoomsEmpty = block_Save.areRoomsEmpty;
        }

        public new House_Save_Data SaveBlock()
        {
            House_Save_Data house_Save = new House_Save_Data(base.SaveBlock(), inhabitants, areRoomsEmpty);
            return house_Save;
        }

        private void Update()
        {
            if (inhabitants.Count > 0) TryRemoveWorker(inhabitants[0]);
        }

        public void Sleep()
        {

            List<Worker_Save_Data> updatedInhabitants = new List<Worker_Save_Data>();
            foreach (Worker_Save_Data worker in inhabitants)
            {
                Worker_Save_Data updatedWorker = worker;
                updatedWorker.isTired = false;
                updatedInhabitants.Add(updatedWorker);
            }
            inhabitants = updatedInhabitants;
        }

        public bool TryTakeWorker(Worker worker)
        {
            if (inhabitants.Count < ((HouseData)block_Data).roomLimit && worker.isTired)
            {
                Board parent_Board = worker.GetComponentInParent<Board>();
                inhabitants.Add(worker.SaveBlock());
                parent_Board.RemoveBlock(worker);
                return true;
            }
            return false;
        }

        private void TryRemoveWorker(Worker_Save_Data saved_Worker)
        {
            if (saved_Worker.isTired) return;
            Board parent_Board = this.GetComponentInParent<Board>();
            if (parent_Board.Try_Spawning_Worker_Block_On_Board_With_Save(saved_Worker.base_Block_Save.id, this.transform.position, saved_Worker))
                inhabitants.Remove(saved_Worker);
        }
    }

    [System.Serializable]
    public struct House_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public List<Worker_Save_Data> inhabitants;
        public bool areRoomsEmpty;

        public House_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            List<Worker_Save_Data> p_Inhabitants,
            bool p_AreRoomsEmpty)
        {
            base_Block_Save = p_Base_Block_Save;
            inhabitants = p_Inhabitants;
            areRoomsEmpty = p_AreRoomsEmpty;
        }
    }
}
