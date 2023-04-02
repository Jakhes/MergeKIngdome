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
        [SerializeField] private Block farm_Prefab;
        [SerializeField] private Block food_Prefab;
        [SerializeField] private Block generator_Prefab;
        [SerializeField] private Block house_Prefab;
        [SerializeField] private Block refiner_Prefab;
        [SerializeField] private Block shop_Prefab;
        [SerializeField] private Block storage_Prefab;
        [SerializeField] private Block unlockable_Prefab;
        [SerializeField] private Block upgradeable_Prefab;
        [SerializeField] private Block worker_Prefab;
        [SerializeField] private Block workstation_Prefab;


        internal Block Create_Block(int block_ID)
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
                    block = Instantiate(empty_Block_Prefab, new Vector3(), Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
                case BlockType.Resource:
                    block = Instantiate(block_Prefab, new Vector3(), Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
                case BlockType.Food:
                    block = Instantiate(food_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Food>().init_Block((FoodData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Farm:
                    block = Instantiate(farm_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Farm>().init_Block((FarmData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Generator:
                    block = Instantiate(generator_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Generator>().init_Block((GeneratorData)block_Info, block_Travel_Time);
                    break;
                case BlockType.House:
                    block = Instantiate(house_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<House>().init_Block((HouseData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Refiner:
                    block = Instantiate(refiner_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Refiner>().init_Block((RefinerData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Shop:
                    block = Instantiate(shop_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Shop>().init_Block((ShopData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Storage:
                    block = Instantiate(storage_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Storage>().init_Block((StorageData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Unlockable:
                    block = Instantiate(unlockable_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Unlockable>().init_Block((UnlockableData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Upgradeable:
                    block = Instantiate(upgradeable_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Upgradeable>().init_Block((UpgradeableData)block_Info, block_Travel_Time);
                    break;
                case BlockType.Worker:
                    block = Instantiate(worker_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Worker>().init_Block((WorkerData)block_Info, block_Travel_Time);
                    break;
                case BlockType.WorkStation:
                    block = Instantiate(workstation_Prefab, new Vector3(), Quaternion.identity);
                    block.GetComponent<Workstation>().init_Block((WorkStationData)block_Info, block_Travel_Time);
                    break;
                default:
                    block = Instantiate(block_Prefab, new Vector3(), Quaternion.identity);
                    block.init_Block(block_Info, block_Travel_Time);
                    break;
            }
            return block;
        }

        internal Block Load_Block_From_Save(Block_Save_Data save_Data)
        {
            BlockData block_Info = GetBlock_Data_By_ID(save_Data.id);

            Block block;
            block = Instantiate(block_Prefab, Vector3.zero, Quaternion.identity);
            block.init_Block(block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Farm_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(farm_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Farm>().init_Block((FarmData)block_Info, block_Travel_Time, save_Data);

            return block;
        }
        internal Block Load_Block_From_Save(Food_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(food_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Food>().init_Block((FoodData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Generator_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(generator_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Generator>().init_Block((GeneratorData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(House_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(house_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<House>().init_Block((HouseData)block_Info, block_Travel_Time, save_Data);

            return block;
        }
        internal Block Load_Block_From_Save(Refiner_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(refiner_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Refiner>().init_Block((RefinerData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Shop_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(shop_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Shop>().init_Block((ShopData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Storage_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(storage_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Storage>().init_Block((StorageData)block_Info, block_Travel_Time, save_Data);

            return block;
        }
        internal Block Load_Block_From_Save(Unlockable_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(unlockable_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Unlockable>().init_Block((UnlockableData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Upgradeable_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(upgradeable_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Upgradeable>().init_Block((UpgradeableData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(Worker_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(worker_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Worker>().init_Block((WorkerData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        internal Block Load_Block_From_Save(WorkStation_Save_Data save_Data)
        {

            BlockData block_Info = GetBlock_Data_By_ID(save_Data.base_Block_Save.id);

            Block block;
            block = Instantiate(workstation_Prefab, Vector3.zero, Quaternion.identity);
            block.GetComponent<Workstation>().init_Block((WorkStationData)block_Info, block_Travel_Time, save_Data);

            return block;
        }

        public Block Create_Empty_Block()
        {
            return Create_Block(0);
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
        Shop,
        Farm,
        Storage,
        Unlockable
    }
}
