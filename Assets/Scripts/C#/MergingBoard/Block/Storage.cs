using System;
using System.Collections;
using System.Collections.Generic;
using EvolvingCode.IngameMessages;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Storage : Block
    {
        [SerializeField] public int currently_Stored_Block_ID;
        [SerializeField] public int stored_Amount;

        public bool TryStoreBlock(Block p_Block)
        {
            if ((currently_Stored_Block_ID == 0 || currently_Stored_Block_ID == p_Block.block_Data.id) &&
                stored_Amount < ((StorageData)block_Data).max_Storage &&
                ((StorageData)block_Data).allowed_Types.Contains(p_Block.block_Data.blockType))
            {
                currently_Stored_Block_ID = p_Block.block_Data.id;
                stored_Amount += 1;
                return true;
            }
            if (!((StorageData)block_Data).allowed_Types.Contains(p_Block.block_Data.blockType))
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.Invalid(transform.position);
            }

            if (stored_Amount >= ((StorageData)block_Data).max_Storage)
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.Full();
            }
            return false;
        }

        public void TryEmptyStorage()
        {
            Board l_Parent_Board = this.GetComponentInParent<Board>();
            for (int i = stored_Amount; i > 0; i--)
            {
                if (l_Parent_Board.Try_Spawning_Block_On_Board(currently_Stored_Block_ID, Parent_Node.transform.position))
                {
                    stored_Amount -= 1;
                }
            }
            CheckIfStorageEmpty();
        }

        private void CheckIfStorageEmpty()
        {
            if (stored_Amount <= 0)
            {
                currently_Stored_Block_ID = 0;
            }
        }


        public void init_Block(StorageData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currently_Stored_Block_ID = 0;
            stored_Amount = 0;
        }

        public void init_Block(StorageData init_Block_Data, float p_Travel_Time, Storage_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            currently_Stored_Block_ID = block_Save.stored_Block_ID;
            stored_Amount = block_Save.stored_Amount;
        }

        public new Storage_Save_Data SaveBlock()
        {
            Storage_Save_Data storage_Save = new Storage_Save_Data(base.SaveBlock(), currently_Stored_Block_ID, stored_Amount);
            return storage_Save;
        }

        public override void DoubleClickAction()
        {
            Board l_Parent_Board = this.GetComponentInParent<Board>();
            if (l_Parent_Board.Try_Spawning_Block_On_Board(currently_Stored_Block_ID, Parent_Node.transform.position))
            {
                stored_Amount -= 1;
            }
            else
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.BoardFull();
            }
            CheckIfStorageEmpty();
        }
    }

    [System.Serializable]
    public struct Storage_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int stored_Block_ID;
        public int stored_Amount;

        public Storage_Save_Data(Block_Save_Data p_Base_Block_Save, int p_Stored_Block_ID, int p_Stored_Amount)
        {
            base_Block_Save = p_Base_Block_Save;
            stored_Block_ID = p_Stored_Block_ID;
            stored_Amount = p_Stored_Amount;
        }
    }
}
