using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EvolvingCode.IngameMessages;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Worker : Block
    {
        [SerializeField] public int currentHealth;
        [SerializeField] public int current_Labor;
        [SerializeField] public bool has_Home;


        public void init_Block(WorkerData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currentHealth = init_Block_Data.maxHP;
            current_Labor = init_Block_Data.max_Labor;
            has_Home = false;
        }

        public void init_Block(WorkerData init_Block_Data, float p_Travel_Time, Worker_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currentHealth = block_Save.current_HP;
            current_Labor = block_Save.current_Labor;
            has_Home = block_Save.has_Home;
        }

        public new Worker_Save_Data SaveBlock()
        {
            Worker_Save_Data workStation_Save = new Worker_Save_Data(base.SaveBlock(), currentHealth, current_Labor, has_Home);
            return workStation_Save;
        }

        public async void NextDay(Vector2 p_House_Pos)
        {
            current_Labor = ((WorkerData)block_Data).max_Labor;

            // Makes the Worker "walk" home to recharge
            await transform.DOMove(p_House_Pos, travel_Time).AsyncWaitForCompletion();
            MoveBlockToNode();

            InfoMessageManager l_InfoMessageManager = this.GetComponentInParent<Board>().InfoMessageManager;
            l_InfoMessageManager.LaborRecharged(((WorkerData)block_Data).max_Labor, Parent_Node.transform.position);
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

        public int UseLabor(int current_Needed_Labor)
        {
            if (current_Labor > 0 && current_Needed_Labor > 0)
            {
                current_Labor--;
                return current_Needed_Labor - 1;
            }
            WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
            l_WarningMessageManager.NotEnoughLabor();
            return current_Needed_Labor;
        }

        internal void LoseHome()
        {
            has_Home = false;
            WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
            l_WarningMessageManager.LostHouse(transform.position);
        }
    }

    [System.Serializable]
    public struct Worker_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int current_HP;
        public int current_Labor;
        public bool has_Home;

        public Worker_Save_Data(Block_Save_Data p_Base_Block_Save, int p_Current_HP, int p_Current_Labor, bool p_Has_Home)
        {
            base_Block_Save = p_Base_Block_Save;
            current_HP = p_Current_HP;
            current_Labor = p_Current_Labor;
            has_Home = p_Has_Home;
        }
    }
}
