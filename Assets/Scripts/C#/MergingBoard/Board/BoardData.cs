using System.Collections.Generic;

namespace EvolvingCode.MergingBoard
{
    [System.Serializable]
    public class BoardData
    {
        public UnityEngine.Vector2 board_Dims;
        public List<Block_Save_Data> block_Saves;
        public List<Food_Save_Data> food_Saves;
        public List<Generator_Save_Data> generator_Saves;
        public List<House_Save_Data> house_Saves;
        public List<Worker_Save_Data> worker_Saves;
        public List<WorkStation_Save_Data> workStation_Saves;
        public List<Upgradeable_Save_Data> upgradeable_Saves;

        // contains the Board Dimensions and Lists of Saves for all BlockTypes
        public BoardData(int width, int height,
            List<Block_Save_Data> p_Block_Saves,
            List<Food_Save_Data> p_Food_Saves,
            List<Generator_Save_Data> p_Generator_Saves,
            List<House_Save_Data> p_House_Saves,
            List<Worker_Save_Data> p_Worker_Saves,
            List<WorkStation_Save_Data> p_WorkStation_Saves,
            List<Upgradeable_Save_Data> p_Upgradeable_Saves)
        {
            board_Dims.x = width;
            board_Dims.y = height;

            block_Saves = p_Block_Saves;
            food_Saves = p_Food_Saves;
            generator_Saves = p_Generator_Saves;
            house_Saves = p_House_Saves;
            worker_Saves = p_Worker_Saves;
            workStation_Saves = p_WorkStation_Saves;
            upgradeable_Saves = p_Upgradeable_Saves;
        }
    }
}
