using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Merger : MonoBehaviour
    {
        [SerializeField] private List<Match> possible_Merges;


        public bool is_Valid_Merge(int a_Block_ID, int b_Block_ID, out int merge_Result_ID)
        {
            int l_Match_ID = possible_Merges.FindIndex(n => n.a_Block.id == a_Block_ID && n.b_Block.id == b_Block_ID
                                                        || n.a_Block.id == b_Block_ID && n.b_Block.id == a_Block_ID);
            if (l_Match_ID >= 0)
            {
                merge_Result_ID = possible_Merges[l_Match_ID].result_Block.id;
                return true;
            }
            merge_Result_ID = 0;
            return false;
        }
    }

    [System.Serializable]
    public struct Match
    {
        public BlockData a_Block;
        public BlockData b_Block;
        public BlockData result_Block;
    }
}
