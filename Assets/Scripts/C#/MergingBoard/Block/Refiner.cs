using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Refiner : Block
    {
        [SerializeField] private RefiningRecipe _Selected_Recipe;
        [SerializeField] private bool _Is_A_Recipe_Selected;
        [SerializeField] private int _Still_Needed_Labor;
        [SerializeField] private List<int> _Item_Buffer;

        private void Update()
        {
            if (_Item_Buffer.Count > 0)
            {
                TryEmptyStorage();
            }
        }


        public bool TrySelectingRecipe(int p_Selecting_Base_Block_ID)
        {
            int l_Found_Index = ((RefinerData)block_Data).refinement_Recipes.FindIndex(x => x.base_Block_To_Refine.id == p_Selecting_Base_Block_ID);

            if (l_Found_Index >= 0 && !_Is_A_Recipe_Selected)
            {
                _Selected_Recipe = ((RefinerData)block_Data).refinement_Recipes[l_Found_Index];
                _Is_A_Recipe_Selected = true;
                _Still_Needed_Labor = _Selected_Recipe.needed_Labor;

                TryRefining();

                return true;
            }
            return false;
        }

        public bool TryWorkOnRefinement(Worker p_Worker)
        {
            Debug.Log("Trying Refining!");
            if (_Is_A_Recipe_Selected && _Selected_Recipe.is_Labor_Needed && _Still_Needed_Labor > 0 && !p_Worker.isTired)
            {
                _Still_Needed_Labor -= 1;
                p_Worker.isTired = true;
                TryRefining();
                return true;
            }
            return false;
        }

        private void TryRefining()
        {
            if (_Item_Buffer.Count <= 0 && (!_Selected_Recipe.is_Labor_Needed || _Still_Needed_Labor <= 0) && _Is_A_Recipe_Selected)
            {
                foreach (BlockData l_Result_Blocks in _Selected_Recipe.refinement_Results)
                {
                    _Item_Buffer.Add(l_Result_Blocks.id);
                }
                _Is_A_Recipe_Selected = false;
            }
        }

        public void TryEmptyStorage()
        {
            Board l_Parent_Board = this.GetComponentInParent<Board>();
            List<int> l_Rest_Buffer = new List<int>();
            foreach (int l_ID in _Item_Buffer)
            {
                if (!l_Parent_Board.Try_Spawning_Block_On_Board(l_ID, Parent_Node.transform.position))
                {
                    l_Rest_Buffer.Add(l_ID);
                }
            }
            _Item_Buffer = l_Rest_Buffer;
        }


        public void init_Block(RefinerData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
            _Selected_Recipe = default(RefiningRecipe);
            _Is_A_Recipe_Selected = false;
            _Still_Needed_Labor = 0;
            _Item_Buffer = new List<int>();
        }

        internal void init_Block(RefinerData init_Block_Data, float p_Travel_Time, Refiner_Save_Data save_Data)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
            _Selected_Recipe = save_Data._Selected_Recipe;
            _Is_A_Recipe_Selected = save_Data._Is_A_Recipe_Selected;
            _Still_Needed_Labor = save_Data._Still_Needed_Labor;
            _Item_Buffer = save_Data._Item_Buffer;
        }

        public new Refiner_Save_Data SaveBlock()
        {
            Refiner_Save_Data Refiner_Save = new Refiner_Save_Data(
                base.SaveBlock(), _Selected_Recipe, _Is_A_Recipe_Selected, _Still_Needed_Labor, _Item_Buffer);

            return Refiner_Save;
        }
    }

    [System.Serializable]
    public struct RefiningRecipe
    {
        public BlockData base_Block_To_Refine;
        public List<BlockData> refinement_Results;
        public bool is_Labor_Needed;
        public int needed_Labor;
    }

    [System.Serializable]
    public struct Refiner_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public RefiningRecipe _Selected_Recipe;
        public bool _Is_A_Recipe_Selected;
        public int _Still_Needed_Labor;
        public List<int> _Item_Buffer;

        public Refiner_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            RefiningRecipe p_Selected_Recipe,
            bool p_Is_A_Recipe_Selected,
            int p_Still_Needed_Labor,
            List<int> p_Item_Buffer)
        {
            base_Block_Save = p_Base_Block_Save;
            _Selected_Recipe = p_Selected_Recipe;
            _Is_A_Recipe_Selected = p_Is_A_Recipe_Selected;
            _Still_Needed_Labor = p_Still_Needed_Labor;
            _Item_Buffer = p_Item_Buffer;
        }
    }


}