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
            Match match = possible_Merges.Find(n => n.a_Block == a_Block_ID && n.b_Block == b_Block_ID
                                                        || n.a_Block == b_Block_ID && n.b_Block == a_Block_ID);
            merge_Result_ID = match.result_Block;
            return match.a_Block != 0;
        }
    }

    [System.Serializable]
    public struct Match
    {
        public int a_Block;
        public int b_Block;
        public int result_Block;
    }
}
