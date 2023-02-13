using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Worker : Block
    {
        [SerializeField] public int currentHealth;
        [SerializeField] public bool isTired;
        [SerializeField] public bool isHungry;

        public void init_Block(WorkerData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currentHealth = init_Block_Data.maxHP;
            isTired = false;
            isHungry = false;
        }

        public void init_Block(WorkerData init_Block_Data, float p_Travel_Time, Worker_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currentHealth = block_Save.current_HP;
            isTired = block_Save.isTired;
            isHungry = block_Save.isHungry;
        }

        private void Update()
        {

        }

        public new Worker_Save_Data SaveBlock()
        {
            Worker_Save_Data workStation_Save = new Worker_Save_Data(base.SaveBlock(), currentHealth, isTired, isHungry, ((WorkerData)block_Data).foodConsumption);
            return workStation_Save;
        }

        public void Sleep()
        {

        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public struct Worker_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int current_HP;
        public bool isTired;
        public bool isHungry;
        public int foodConsumption;

        public Worker_Save_Data(Block_Save_Data p_Base_Block_Save, int p_Current_HP, bool p_IsTired, bool p_IsHungry, int p_FoodConsumption)
        {
            base_Block_Save = p_Base_Block_Save;
            current_HP = p_Current_HP;
            isTired = p_IsTired;
            isHungry = p_IsHungry;
            foodConsumption = p_FoodConsumption;
        }
    }
}
