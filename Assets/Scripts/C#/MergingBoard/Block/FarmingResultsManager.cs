using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class FarmingResultsManager : MonoBehaviour
    {
        [SerializeField] private List<FarmingResult> _CropFarming;
        [SerializeField] private List<FarmingResult> _AnimalFarming;
        [SerializeField] private List<FarmingResult> _MonsterFarming;

        internal bool CheckIfValidSlotItem(int p_Potential_Slot_Block_ID, FarmType farmType)
        {
            List<FarmingResult> l_Selected_Results;
            switch (farmType)
            {
                case FarmType.CropFarm:
                    l_Selected_Results = _CropFarming;
                    break;
                case FarmType.AnimalFarm:
                    l_Selected_Results = _AnimalFarming;
                    break;
                case FarmType.MonsterFarm:
                    l_Selected_Results = _MonsterFarming;
                    break;
                default:
                    l_Selected_Results = _CropFarming;
                    break;
            }

            int id = l_Selected_Results.FindIndex(x => x._Base_Block.id == p_Potential_Slot_Block_ID);
            return (id >= 0);
        }

        internal List<int> GetFarmingResultsForIDs(List<int> slot_Entries, FarmType farmType)
        {
            List<FarmingResult> l_Selected_Results;
            switch (farmType)
            {
                case FarmType.CropFarm:
                    l_Selected_Results = _CropFarming;
                    break;
                case FarmType.AnimalFarm:
                    l_Selected_Results = _AnimalFarming;
                    break;
                case FarmType.MonsterFarm:
                    l_Selected_Results = _MonsterFarming;
                    break;
                default:
                    l_Selected_Results = _CropFarming;
                    break;
            }

            List<int> l_Results = new List<int>();
            foreach (int l_Slot_Entry_ID in slot_Entries)
            {
                int id = l_Selected_Results.FindIndex(x => x._Base_Block.id == l_Slot_Entry_ID);
                foreach (BlockData l_Result_Data in l_Selected_Results[id]._Results)
                {
                    l_Results.Add(l_Result_Data.id);
                }
            }

            return l_Results;
        }
    }

    [System.Serializable]
    public struct FarmingResult
    {
        public BlockData _Base_Block;
        public List<BlockData> _Results;
    }
}