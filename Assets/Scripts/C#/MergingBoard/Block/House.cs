using System;
using System.Collections;
using System.Collections.Generic;
using EvolvingCode.IngameMessages;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class House : Block
    {
        [SerializeField] public List<Worker> inhabitants;
        [SerializeField] public List<Worker_Save_Data> saved_Inhabitants;
        [SerializeField] public bool areRoomsEmpty;


        public void init_Block(HouseData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = new List<Worker>();
            saved_Inhabitants = new List<Worker_Save_Data>();
            areRoomsEmpty = true;
        }

        public void init_Block(HouseData init_Block_Data, float p_Travel_Time, House_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = new List<Worker>();
            saved_Inhabitants = block_Save.inhabitants;
            areRoomsEmpty = block_Save.areRoomsEmpty;
        }

        public new House_Save_Data SaveBlock()
        {
            inhabitants.ForEach(x => saved_Inhabitants.Add(x.SaveBlock()));
            House_Save_Data house_Save = new House_Save_Data(base.SaveBlock(), saved_Inhabitants, areRoomsEmpty);
            saved_Inhabitants = new List<Worker_Save_Data>();
            return house_Save;
        }

        private void Update()
        {
            if (saved_Inhabitants.Count > 0) TrySpawnSavedWorker(saved_Inhabitants[0]);
        }

        public void NextDay()
        {
            inhabitants.ForEach(x => x.NextDay(this.transform.position));
        }

        public bool TryAssignWorker(Worker p_Worker)
        {
            if (inhabitants.Count < ((HouseData)block_Data).roomLimit && !inhabitants.Contains(p_Worker) && !p_Worker.has_Home)
            {
                inhabitants.Add(p_Worker);
                p_Worker.has_Home = true;

                InfoMessageManager l_InfoMessageManager = this.GetComponentInParent<Board>().InfoMessageManager;
                l_InfoMessageManager.AssignHouse();
            }
            else if (inhabitants.Count >= ((HouseData)block_Data).roomLimit)
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.HouseFull();
            }
            else if (inhabitants.Contains(p_Worker) || p_Worker.has_Home)
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.AlreadyAssigned();
            }
            return false;
        }

        public void TryRemoveWorker(Worker p_Worker)
        {
            if (inhabitants.Contains(p_Worker))
            {
                inhabitants.Remove(p_Worker);
                p_Worker.LoseHome();
            }
        }

        private void TrySpawnSavedWorker(Worker_Save_Data saved_Worker)
        {
            Board parent_Board = this.GetComponentInParent<Board>();
            Worker worker = parent_Board.Try_Spawning_Worker_Block_On_Board_With_Save_And_Return(saved_Worker.base_Block_Save.id, this.transform.position, saved_Worker);
            if (worker != null)
            {
                saved_Inhabitants.Remove(saved_Worker);
                inhabitants.Add(worker);
            }
        }

        public override void SellBlock()
        {
            inhabitants.ForEach(x => x.LoseHome());
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
