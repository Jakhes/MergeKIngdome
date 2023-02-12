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
}
