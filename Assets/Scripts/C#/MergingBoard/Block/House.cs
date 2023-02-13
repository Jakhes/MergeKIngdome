using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class House : Block
    {
        [SerializeField] public List<Worker_Save_Data> inhabitants;
        [SerializeField] public List<Food_Save_Data> foodStorage;
        [SerializeField] public bool isFoodStorageEmpty;
        [SerializeField] public bool areRoomsEmpty;


        public void init_Block(HouseData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = new List<Worker_Save_Data>();
            foodStorage = new List<Food_Save_Data>();
            isFoodStorageEmpty = true;
            areRoomsEmpty = true;
        }

        public void init_Block(HouseData init_Block_Data, float p_Travel_Time, House_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            inhabitants = block_Save.inhabitants;
            foodStorage = block_Save.foodStorage;
            isFoodStorageEmpty = block_Save.isFoodStorageEmpty;
            areRoomsEmpty = block_Save.areRoomsEmpty;
        }

        public new House_Save_Data SaveBlock()
        {
            House_Save_Data house_Save = new House_Save_Data(base.SaveBlock(), inhabitants, foodStorage, isFoodStorageEmpty, areRoomsEmpty);
            return house_Save;
        }

        private void Update()
        {
            if (inhabitants.Count > 0) TryRemoveWorker(inhabitants[0]);
        }

        public void Sleep()
        {
            TryFeedInhabitants();

            List<Worker_Save_Data> updatedInhabitants = new List<Worker_Save_Data>();
            foreach (Worker_Save_Data worker in inhabitants)
            {
                if (!worker.isHungry)
                {
                    Worker_Save_Data updatedWorker = worker;
                    updatedWorker.isTired = false;
                    updatedInhabitants.Add(updatedWorker);
                }
                else updatedInhabitants.Add(worker);
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

        public bool TryTakeFood(Food food)
        {
            if (foodStorage.Count < ((HouseData)block_Data).foodStorageLimit)
            {
                Board parent_Board = food.GetComponentInParent<Board>();
                foodStorage.Add(food.SaveBlock());
                parent_Board.RemoveBlock(food);
                return true;
            }
            return false;
        }

        private void TryRemoveFood(int slotId)
        {
            Food_Save_Data savedFood = foodStorage[slotId];
            Board parent_Board = this.GetComponentInParent<Board>();
            if (parent_Board.Try_Spawning_Food_Block_On_Board_With_Save(savedFood.base_Block_Save.id, this.transform.position, savedFood))
                foodStorage.Remove(savedFood);

        }

        private void TryFeedInhabitants()
        {
            List<Worker_Save_Data> updatedInhabitants = new List<Worker_Save_Data>();
            foreach (Worker_Save_Data worker in inhabitants)
            {
                int remainingConsumption = worker.foodConsumption;
                if (worker.isHungry)
                {
                    for (int i = 0; i < worker.foodConsumption; i++)
                    {
                        if (foodStorage.Count > 0)
                        {
                            Food_Save_Data firstFood = foodStorage[0];
                            firstFood.chargesLeft -= 1;
                            remainingConsumption -= 1;
                            if (firstFood.chargesLeft <= 0)
                            {
                                foodStorage.RemoveAt(0);
                            }
                            else
                            {
                                foodStorage[0] = firstFood;
                            }
                        }
                    }
                }
                if (remainingConsumption == 0)
                {
                    Worker_Save_Data updatedWorker = worker;
                    updatedWorker.isHungry = false;
                    updatedInhabitants.Add(updatedWorker);
                }
                else updatedInhabitants.Add(worker);
            }
            inhabitants = updatedInhabitants;
        }
    }

    [System.Serializable]
    public struct House_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public List<Worker_Save_Data> inhabitants;
        public List<Food_Save_Data> foodStorage;
        public bool isFoodStorageEmpty;
        public bool areRoomsEmpty;

        public House_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            List<Worker_Save_Data> p_Inhabitants,
            List<Food_Save_Data> p_FoodStorage,
            bool p_IsFoodEmpty,
            bool p_AreRoomsEmpty)
        {
            base_Block_Save = p_Base_Block_Save;
            inhabitants = p_Inhabitants;
            foodStorage = p_FoodStorage;
            isFoodStorageEmpty = p_IsFoodEmpty;
            areRoomsEmpty = p_AreRoomsEmpty;
        }
    }
}
