using System.Collections.Generic;

namespace EvolvingCode.MergingBoard
{
    [System.Serializable]
    public class BoardData
    {
        public UnityEngine.Vector2 board_Dims;
        public List<Block_Save_Data> block_Saves;
        public List<Farm_Save_Data> farm_Saves;
        public List<Food_Save_Data> food_Saves;
        public List<Generator_Save_Data> generator_Saves;
        public List<House_Save_Data> house_Saves;
        public List<Refiner_Save_Data> refiner_Saves;
        public List<Shop_Save_Data> shop_Saves;
        public List<Storage_Save_Data> storage_Saves;
        public List<Unlockable_Save_Data> unlockable_Saves;
        public List<Upgradeable_Save_Data> upgradeable_Saves;
        public List<Worker_Save_Data> worker_Saves;
        public List<WorkStation_Save_Data> workStation_Saves;

        // contains the Board Dimensions and Lists of Saves for all BlockTypes
        public BoardData(int width, int height,
            List<Block_Save_Data> p_Block_Saves,
            List<Farm_Save_Data> p_farm_Saves,
            List<Food_Save_Data> p_Food_Saves,
            List<Generator_Save_Data> p_Generator_Saves,
            List<House_Save_Data> p_House_Saves,
            List<Refiner_Save_Data> p_Refiner_Saves,
            List<Shop_Save_Data> p_Shop_Saves,
            List<Storage_Save_Data> p_Storage_Saves,
            List<Unlockable_Save_Data> p_Unlockable_Saves,
            List<Upgradeable_Save_Data> p_Upgradeable_Saves,
            List<Worker_Save_Data> p_Worker_Saves,
            List<WorkStation_Save_Data> p_WorkStation_Saves)
        {
            board_Dims.x = width;
            board_Dims.y = height;

            block_Saves = p_Block_Saves;
            farm_Saves = p_farm_Saves;
            food_Saves = p_Food_Saves;
            generator_Saves = p_Generator_Saves;
            house_Saves = p_House_Saves;
            refiner_Saves = p_Refiner_Saves;
            shop_Saves = p_Shop_Saves;
            storage_Saves = p_Storage_Saves;
            unlockable_Saves = p_Unlockable_Saves;
            upgradeable_Saves = p_Upgradeable_Saves;
            worker_Saves = p_Worker_Saves;
            workStation_Saves = p_WorkStation_Saves;
        }
    }
}
