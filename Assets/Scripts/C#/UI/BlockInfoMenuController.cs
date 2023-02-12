using System.Collections;
using System.Collections.Generic;
using EvolvingCode.MergingBoard;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace EvolvingCode
{
    public class BlockInfoMenuController : MonoBehaviour
    {
        public Board current_Board;
        public GameManager game_Manager;
        [SerializeField] private BlockManager _BlockManager;
        [SerializeField] private Block current_Selected_Block;

        [SerializeField] private bool _locked;
        [SerializeField] private RectTransform _rectTransform;

        public Sprite true_Sprite;
        public Sprite false_Sprite;


        // Header References
        [SerializeField] private GameObject _Header_Panel;
        [SerializeField] private Image _Main_Image;
        [SerializeField] private TMP_Text _Name_Text;
        [SerializeField] private TMP_Text _Level_Text;
        [SerializeField] private GameObject _Max_Tag;

        // Description References
        [SerializeField] private GameObject _Description_Panel;
        [SerializeField] private TMP_Text _Description_Text;

        // Charge References
        [SerializeField] private GameObject _Charge_Panel;
        [SerializeField] private TMP_Text _Days_charge_Text;
        [SerializeField] private TMP_Text _Remaining_Text;
        [SerializeField] private TMP_Text _Stored_Text;

        // House References
        [SerializeField] private GameObject _House_Info_Panel;
        [SerializeField] private TMP_Text _Room_Limit_Text;
        [SerializeField] private GameObject _Inhabitants_Scroll_Content_Obj;
        [SerializeField] private TMP_Text _Storage_Limit_Text;
        [SerializeField] private GameObject _Food_Storage_Scroll_Content_Obj;

        // Worker References
        [SerializeField] private GameObject _Worker_Extra_Info_Panel;
        [SerializeField] private TMP_Text _Job_Text;
        [SerializeField] private TMP_Text _Health_Text;
        [SerializeField] private TMP_Text _Damage_Text;
        [SerializeField] private TMP_Text _Defense_Text;
        [SerializeField] private TMP_Text _Food_Consumption_Text;
        [SerializeField] private Image _Tired_Image;
        [SerializeField] private TMP_Text _Tired_Text;
        [SerializeField] private Image _Hungry_Image;
        [SerializeField] private TMP_Text _Hungry_Text;

        // Workstation References
        [SerializeField] private GameObject _Workstation_Info_Panel;
        [SerializeField] private Image _IsLimitless_Image;
        [SerializeField] private TMP_Text _IsLimitless_Text;
        [SerializeField] private TMP_Text _ChargesLeft_Text;
        [SerializeField] private TMP_Text _Days_Per_Charge_Text;
        [SerializeField] private TMP_Text _Items_Per_Charge_Text;
        [SerializeField] private TMP_Text _Storage_Text;
        [SerializeField] private Image _HasWorker_Image;
        [SerializeField] private TMP_Text _HasWorker_Text;

        // Results References
        [SerializeField] private GameObject _Results_Panel;
        [SerializeField] private GameObject _Scroll_Content_Obj;
        [SerializeField] private Image _Content_Image_Prefab;

        // Button References
        [SerializeField] private GameObject _Button_Panel;
        [SerializeField] private GameObject _Sell_Button;
        [SerializeField] private GameObject _Generate_All_Button;

        private void Update()
        {
            if (current_Selected_Block != null)
            {
                UpdateMenu(current_Selected_Block);
            }
            else
            {
                DeactivateMenu();
            }

            if (!_locked)
            {
                Vector2 M_ouse_Position = Input.mousePosition;

                float pivotX = M_ouse_Position.x / Screen.width;
                float pivotY = M_ouse_Position.y / Screen.height;


                _rectTransform.pivot = new Vector2(pivotX, pivotY);

                transform.position = M_ouse_Position;
            }
        }

        public void UpdateMenu(Block selected_Block)
        {
            BlockData block_Data = selected_Block.block_Data;

            switch (selected_Block.block_Data.blockType)
            {
                case BlockType.Empty:
                    return;
                case BlockType.Resource:
                    ResourceSelection(selected_Block, block_Data);
                    break;
                case BlockType.Generator:
                    GeneratorSelection((Generator)selected_Block, (GeneratorData)block_Data);
                    break;
                case BlockType.House:
                    HouseSelection((House)selected_Block, (HouseData)block_Data);
                    break;
                case BlockType.Worker:
                    WorkerSelection((Worker)selected_Block, (WorkerData)block_Data);
                    break;
                case BlockType.WorkStation:
                    WorkStationSelection((Workstation)selected_Block, (WorkStationData)block_Data);
                    break;
                default:
                    ResourceSelection(selected_Block, block_Data);
                    break;
            }

            current_Selected_Block = selected_Block;
        }


        private void ResourceSelection(Block block, BlockData blockData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data
            // Header
            _Main_Image.sprite = blockData.sprite;
            _Name_Text.text = blockData.name;
            _Level_Text.text = blockData.blockType.ToString() + "\nLvl " + blockData.level;
            _Max_Tag.SetActive(blockData.isMaxLevel);
            // Description
            _Description_Text.text = blockData.description;

            // Activate useable Buttons
            _Sell_Button.SetActive(true);
            _Generate_All_Button.SetActive(false);
        }

        private void GeneratorSelection(Generator generator, GeneratorData generatorData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(true);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Results_Panel.SetActive(true);
            _Button_Panel.SetActive(true);

            // Fill in the Data
            // Header
            _Main_Image.sprite = generatorData.sprite;
            _Name_Text.text = generatorData.name;
            _Level_Text.text = generatorData.blockType.ToString() + "\nLvl " + generatorData.level;
            _Max_Tag.SetActive(generatorData.isMaxLevel);
            // Description
            _Description_Text.text = generatorData.description;
            // Charge
            _Days_charge_Text.text = "Days/Charge : " + generatorData.charge_Time;
            _Remaining_Text.text = generator.remaining_Time + " Days remaining";
            _Stored_Text.text = generator.item_Buffer.Count + " / " + generatorData.max_ItemBuffer + " Stored";

            // Results
            int result_Amount = generatorData.possible_Results.Count;
            int content_Window_Width = result_Amount * 130 + 40;
            _Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(content_Window_Width, 110);
            List<Image> images = _Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Take(result_Amount)
                .ToList();

            for (int i = 0; i < result_Amount; i++)
            {
                images[i].gameObject.SetActive(true);
                Sprite sprite = generatorData.possible_Results.list.ToList()[i].item.sprite;
                images[i].sprite = sprite;
            }
            List<Image> unused_Images = _Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Where(n => !images.Contains(n)).ToList();
            foreach (var item in unused_Images)
            {
                item.gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(true);
            // if it is a neighbor generator deactivate generate all button
            _Generate_All_Button.SetActive(!generatorData.isNeighborGenerator);

        }

        private void WorkerSelection(Worker worker, WorkerData worker_Data)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(true);
            _Workstation_Info_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Button_Panel.SetActive(false);

            // Fill in the Data
            // Header
            _Main_Image.sprite = worker_Data.sprite;
            _Name_Text.text = worker_Data.name;
            _Level_Text.text = worker_Data.blockType.ToString() + "\nLvl " + worker_Data.level;
            _Max_Tag.SetActive(worker_Data.isMaxLevel);
            // Description
            _Description_Text.text = worker_Data.description;

            // Worker Extra Info
            _Job_Text.text = "Job : " + worker_Data.job.ToString();
            _Health_Text.text = worker.currentHealth + " / " + worker_Data.maxHP + " HP";
            _Damage_Text.text = worker_Data.attackDamage + " Damage";
            _Defense_Text.text = worker_Data.defense + " Defense";
            _Food_Consumption_Text.text = "Consumes :" + worker_Data.foodConsumption + " Food Charges";

            if (worker.isTired)
                _Tired_Image.sprite = true_Sprite;
            else
                _Tired_Image.sprite = false_Sprite;

            if (worker.isHungry)
                _Hungry_Image.sprite = true_Sprite;
            else
                _Hungry_Image.sprite = false_Sprite;

            // Activate useable Buttons
            _Sell_Button.SetActive(false);
            _Generate_All_Button.SetActive(false);
        }

        private void WorkStationSelection(Workstation p_Workstation, WorkStationData p_WorkstationData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(true);
            _Results_Panel.SetActive(true);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header
            _Main_Image.sprite = p_WorkstationData.sprite;
            _Name_Text.text = p_WorkstationData.name;
            _Level_Text.text = p_WorkstationData.blockType.ToString() + "\nLvl " + p_WorkstationData.level;
            _Max_Tag.SetActive(p_WorkstationData.isMaxLevel);

            // Description
            _Description_Text.text = p_WorkstationData.description;

            // Worker Extra Info
            if (p_WorkstationData.isLimitless)
                _IsLimitless_Image.sprite = true_Sprite;
            else
                _IsLimitless_Image.sprite = false_Sprite;

            _ChargesLeft_Text.text = "Charges Left: " + p_Workstation.remainingCharges;
            _Days_Per_Charge_Text.text = (p_WorkstationData.productionDays - p_Workstation.currentProductionDay + 1) + " Work Units until next Charge";
            _Items_Per_Charge_Text.text = p_WorkstationData.items_Per_Charge + " Items per Charge";
            _Items_Per_Charge_Text.text = p_Workstation.item_Buffer.Count + " / " + p_WorkstationData.storageLimit + " Stored";

            if (p_Workstation.hasWorker)
                _HasWorker_Image.sprite = true_Sprite;
            else
                _HasWorker_Image.sprite = false_Sprite;

            // Results
            int result_Amount = p_WorkstationData.possible_Results.Count;
            int content_Window_Width = result_Amount * 130 + 40;
            _Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(content_Window_Width, 110);
            List<Image> images = _Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Take(result_Amount)
                .ToList();

            for (int i = 0; i < result_Amount; i++)
            {
                images[i].gameObject.SetActive(true);
                Sprite sprite = p_WorkstationData.possible_Results.list.ToList()[i].item.sprite;
                images[i].sprite = sprite;
            }
            List<Image> unused_Images = _Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Where(n => !images.Contains(n)).ToList();
            foreach (var item in unused_Images)
            {
                item.gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(true);
            _Generate_All_Button.SetActive(false);

        }

        private void HouseSelection(House p_House, HouseData p_HouseData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(true);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header
            _Main_Image.sprite = p_HouseData.sprite;
            _Name_Text.text = p_HouseData.name;
            _Level_Text.text = p_HouseData.blockType.ToString() + "\nLvl " + p_HouseData.level;
            _Max_Tag.SetActive(p_HouseData.isMaxLevel);

            // Description
            _Description_Text.text = p_HouseData.description;

            // House Info
            _Room_Limit_Text.text = p_House.inhabitants.Count + " / " + p_HouseData.roomLimit + " Room used";
            _Storage_Limit_Text.text = p_House.foodStorage.Count + " / " + p_HouseData.foodStorageLimit + " Storage used";

            // Inhabitants Scroll View
            List<Worker_Save_Data> l_Inhabitants_List = p_House.inhabitants;
            int l_Inhabitants_Amount = l_Inhabitants_List.Count;
            int l_Inhabitants_Content_Window_Width = l_Inhabitants_Amount * 130 + 40;
            _Inhabitants_Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(l_Inhabitants_Content_Window_Width, 110);

            List<Image> l_Images = _Inhabitants_Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Take(l_Inhabitants_Amount)
                .ToList();

            for (int i = 0; i < l_Inhabitants_Amount; i++)
            {
                l_Images[i].canvasRenderer.SetAlpha(1);
                int l_Worker_Block_ID = l_Inhabitants_List[i].base_Block_Save.id;
                Sprite sprite = _BlockManager.GetBlock_Data_By_ID(l_Worker_Block_ID).sprite;
                l_Images[i].sprite = sprite;
            }
            List<Image> l_Unused_Images = _Inhabitants_Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Where(n => !l_Images.Contains(n)).ToList();
            foreach (var item in l_Unused_Images)
            {
                item.canvasRenderer.SetAlpha(0);
            }

            // Inhabitants Scroll View
            List<Food_Save_Data> l_Food_Storage_List = p_House.foodStorage;
            int l_Food_Amount = l_Food_Storage_List.Count;
            int l_Storage_Content_Window_Width = l_Food_Amount * 130 + 40;
            _Food_Storage_Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(l_Storage_Content_Window_Width, 110);

            List<Image> l_Storage_Images = _Food_Storage_Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Take(l_Food_Amount)
                .ToList();

            for (int i = 0; i < l_Food_Amount; i++)
            {
                l_Storage_Images[i].canvasRenderer.SetAlpha(1);
                int l_Worker_Block_ID = l_Food_Storage_List[i].base_Block_Save.id;
                Sprite sprite = _BlockManager.GetBlock_Data_By_ID(l_Worker_Block_ID).sprite;
                l_Storage_Images[i].sprite = sprite;
            }
            List<Image> l_Storage_Unused_Images = _Food_Storage_Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Where(n => !l_Storage_Images.Contains(n)).ToList();
            foreach (var item in l_Storage_Unused_Images)
            {
                item.canvasRenderer.SetAlpha(0);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(true);
            _Generate_All_Button.SetActive(false);

        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }

        public void SellBlock()
        {
            game_Manager.SellBlock(current_Selected_Block);
            this.gameObject.SetActive(false);
        }

        public void GenerateAll()
        {
            ((Generator)current_Selected_Block).TryEmptyStorage();
        }
    }
}
