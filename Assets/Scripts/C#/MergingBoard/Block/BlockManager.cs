using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private float block_Travel_Time = 0.5f;
        [SerializeField] private List<BlockData> blocks;
        [SerializeField] private Block empty_Block_Prefab;
        [SerializeField] private Block block_Prefab;
        [SerializeField] private Block food_Prefab;
        [SerializeField] private Block generator_Prefab;
        [SerializeField] private Block house_Prefab;
        [SerializeField] private Block refiner_Prefab;
        [SerializeField] private Block shop_Prefab;
        [SerializeField] private Block upgradeable_Prefab;
        [SerializeField] private Block worker_Prefab;
        [SerializeField] private Block workstation_Prefab;


        internal Block Create_Block(int block_ID, Node parent_Node)
        {
            Block block;
            BlockData block_Info = blocks.Find(n => n.id == block_ID);
            if (block_Info == null)
            {
                Debug.Log("No Block Found with Id: " + block_ID);
                return null;
            }
            switch (block_Info.blockType)
            {
                case BlockType.Empty:
                    block = Instantiate(empty_Block_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
                case BlockType.Resource:
                    block = Instantiate(block_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
                case BlockType.Food:
                    block = Instantiate(food_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Food>().init_Block((FoodData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Generator:
                    block = Instantiate(generator_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Generator>().init_Block((GeneratorData)block_Info, block_Travel_Time);
                    break;
                case BlockType.House:
                    block = Instantiate(house_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<House>().init_Block((HouseData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Refiner:
                    block = Instantiate(refiner_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Refiner>().init_Block((RefinerData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Shop:
                    block = Instantiate(shop_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Shop>().init_Block((ShopData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Upgradeable:
                    block = Instantiate(upgradeable_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Upgradeable>().init_Block((UpgradeableData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Worker:
                    block = Instantiate(worker_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Worker>().init_Block((WorkerData)block_Info, block_Travel_Time);
                    break;
                case BlockType.WorkStation:
                    block = Instantiate(workstation_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.GetComponent<Workstation>().init_Block((WorkStationData)block_Info, block_Travel_Time);
                    break;
                default:
                    block = Instantiate(block_Prefab, parent_Node.transform.position, Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
            }

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Block_Save_Data save_Data, Node parent_Node)
        {
            BlockData block_Info = GetBlock_Data_By_ID(save_Data.id);

            Block block;
            block = Instantiate(block_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.init_Block(block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Food_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(food_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Food>().init_Block((FoodData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Generator_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(generator_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Generator>().init_Block((GeneratorData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(House_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(house_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<House>().init_Block((HouseData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }
        internal Block Load_Block_From_Save(Refiner_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(refiner_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Refiner>().init_Block((RefinerData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Shop_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(shop_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Shop>().init_Block((ShopData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Worker_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(worker_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Worker>().init_Block((WorkerData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(WorkStation_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(workstation_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Workstation>().init_Block((WorkStationData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        internal Block Load_Block_From_Save(Upgradeable_Save_Data save_Data, Node parent_Node)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(upgradeable_Prefab, parent_Node.transform.position, Quaternion.identity);
            block.GetComponent<Upgradeable>().init_Block((UpgradeableData)block_Info, block_Travel_Time, save_Data);

            Destroy(parent_Node.current_Block.gameObject);

            parent_Node.current_Block = block;
            block.Parent_Node = parent_Node;

            return block;
        }

        public Block Create_Empty_Block(Node node)
        {
            return Create_Block(0, node);
        }

        public BlockData GetBlock_Data_By_ID(int id)
        {
            return blocks.Find(n => n.id == id);
        }
    }

    public enum BlockType
    {
        Empty,
        Upgradeable,
        Resource,
        Food,
        Generator,
        House,
        Refiner,
        Processor,
        Character,
        Consumable,
        Worker,
        WorkStation,
        Shop
    }

    [System.Serializable]
    public struct Block_Save_Data
    {
        public int id;
        public Vector2 node_Pos;

        public Block_Save_Data(int p_ID, Vector2 p_Node_Pos)
        {
            id = p_ID;
            node_Pos = p_Node_Pos;
        }
    }

    [System.Serializable]
    public struct Food_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int chargesLeft;

        public Food_Save_Data(Block_Save_Data p_Base_Block_Save, int p_ChargesLeft)
        {
            base_Block_Save = p_Base_Block_Save;
            chargesLeft = p_ChargesLeft;
        }
    }

    [System.Serializable]
    public struct Generator_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public bool isCharging;
        public int remaining_Time;
        public List<int> item_Buffer;

        public Generator_Save_Data(Block_Save_Data p_Base_Block_Save, bool p_IsCharging, int p_Remaining_Time, List<int> p_Item_Buffer)
        {
            base_Block_Save = p_Base_Block_Save;
            isCharging = p_IsCharging;
            remaining_Time = p_Remaining_Time;
            item_Buffer = p_Item_Buffer;
        }
    }

    [System.Serializable]
    public struct House_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public List<Worker_Save_Data> inhabitants;
        public List<Food_Save_Data> foodStorage;
        public bool isFoodStorageEmpty;
        public bool areRoomsEmpty;

        public House_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            List<Worker_Save_Data> p_Inhabitants,
            List<Food_Save_Data> p_FoodStorage,
            bool p_IsFoodEmpty,
            bool p_AreRoomsEmpty)
        {
            base_Block_Save = p_Base_Block_Save;
            inhabitants = p_Inhabitants;
            foodStorage = p_FoodStorage;
            isFoodStorageEmpty = p_IsFoodEmpty;
            areRoomsEmpty = p_AreRoomsEmpty;
        }
    }


    [System.Serializable]
    public struct Shop_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public Shop_Save_Data(Block_Save_Data p_Base_Block_Save)
        {
            base_Block_Save = p_Base_Block_Save;
        }
    }

    [System.Serializable]
    public struct Worker_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int current_HP;
        public bool isTired;
        public bool isHungry;
        public int foodConsumption;

        public Worker_Save_Data(Block_Save_Data p_Base_Block_Save, int p_Current_HP, bool p_IsTired, bool p_IsHungry, int p_FoodConsumption)
        {
            base_Block_Save = p_Base_Block_Save;
            current_HP = p_Current_HP;
            isTired = p_IsTired;
            isHungry = p_IsHungry;
            foodConsumption = p_FoodConsumption;
        }
    }

    [System.Serializable]
    public struct WorkStation_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public int remaining_Charges;
        public int current_Production_Day;
        public List<int> item_Buffer;
        public bool hasWorker;
        public Worker_Save_Data savedWorker;

        public WorkStation_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            int p_Remaining_Charges,
            int p_Current_Production_Day,
            List<int> p_Item_Buffer,
            bool p_HasWorker,
            Worker_Save_Data p_SavedWorker)
        {
            base_Block_Save = p_Base_Block_Save;
            remaining_Charges = p_Remaining_Charges;
            current_Production_Day = p_Current_Production_Day;
            item_Buffer = p_Item_Buffer;
            hasWorker = p_HasWorker;
            savedWorker = p_SavedWorker;
        }
    }

    [System.Serializable]
    public struct Upgradeable_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public List<UpgradeMaterial> saved_Materials;
        public bool is_Upgrade_Ready;

        public Upgradeable_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            List<UpgradeMaterial> p_Saved_Materials,
            bool p_Is_Upgrade_Ready)
        {
            base_Block_Save = p_Base_Block_Save;
            saved_Materials = p_Saved_Materials;
            is_Upgrade_Ready = p_Is_Upgrade_Ready;
        }
    }
}
