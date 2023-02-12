using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Shop : Block
    {
        public void init_Block(ShopData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
        }

        internal void init_Block(ShopData init_Block_Data, float p_Travel_Time, Shop_Save_Data save_Data)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
        }

        public new Shop_Save_Data SaveBlock()
        {
            Shop_Save_Data Shop_Save = new Shop_Save_Data(base.SaveBlock());
            return Shop_Save;
        }

        public int BuyBlock(int p_Block_ID, int p_Current_Gold_Amount)
        {
            ShopEntry l_Selected_Entry = ((ShopData)block_Data).shopEntries.Find(x => x.BlockID == p_Block_ID);
            if (l_Selected_Entry.BlockID == 0)
            {
                Debug.Log("Tried Buying a Block with the ID " + p_Block_ID + ", but there is no Shop Entry with that ID.");
                return p_Current_Gold_Amount;
            }
            else
            {
                if (p_Current_Gold_Amount < l_Selected_Entry.Cost)
                {
                    return p_Current_Gold_Amount;
                }

                Board parent_Board = this.GetComponentInParent<Board>();
                if (!parent_Board.Try_Spawning_Block_On_Board(p_Block_ID, Parent_Node.transform.position))
                {
                    return p_Current_Gold_Amount;
                }

                return p_Current_Gold_Amount - l_Selected_Entry.Cost;
            }
        }


    }

    [System.Serializable]
    public struct ShopEntry
    {
        public int BlockID;
        public int Cost;
    }
}
