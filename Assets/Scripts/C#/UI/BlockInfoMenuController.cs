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
        [SerializeField] public Block current_Selected_Block;

        [SerializeField] private bool _locked;
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Transform _Selection_Indicator;

        public Sprite true_Sprite;
        public Sprite false_Sprite;


        // Header References
        [SerializeField] private GameObject _Header_Panel;
        [SerializeField] private Image _Main_Image;
        [SerializeField] private TMP_Text _Name_Text;
        [SerializeField] private TMP_Text _Level_Text;
        [SerializeField] private GameObject _Max_Tag;
        [SerializeField] private TMP_Text _Value_Text;


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

        // Worker References
        [SerializeField] private GameObject _Worker_Extra_Info_Panel;
        [SerializeField] private TMP_Text _Job_Text;
        [SerializeField] private TMP_Text _Health_Text;
        [SerializeField] private TMP_Text _Damage_Text;
        [SerializeField] private TMP_Text _Defense_Text;
        [SerializeField] private TMP_Text _Labor_Left_Text;
        [SerializeField] private Image _Worker_Has_Home_Image;
        [SerializeField] private TMP_Text _Worker_Has_Home_Text;


        // Workstation References
        [SerializeField] private GameObject _Workstation_Info_Panel;
        [SerializeField] private Image _IsLimitless_Image;
        [SerializeField] private TMP_Text _IsLimitless_Text;
        [SerializeField] private TMP_Text _ChargesLeft_Text;
        [SerializeField] private TMP_Text _Needed_Labor_Text;
        [SerializeField] private TMP_Text _Items_Per_Charge_Text;
        [SerializeField] private TMP_Text _Storage_Text;

        // Results References
        [SerializeField] private GameObject _Results_Panel;
        [SerializeField] private GameObject _Scroll_Content_Obj;
        [SerializeField] private Image _Content_Image_Prefab;

        // Shop References
        [SerializeField] private GameObject _Shop_Panel;
        [SerializeField] private GameObject _Shop_Scroll_Content_Obj;
        [SerializeField] private GameObject _Shop_Entry_Panel_Prefab;
        [SerializeField] private List<ShopUIEntry> _Shop_Entry_Panel_Pool;

        // Upgradeable References
        [SerializeField] private GameObject _Upgradeable_Panel;
        [SerializeField] private UpgradeMaterialUI _Upgrade_Target_Panel;
        [SerializeField] private GameObject _Upgradeable_Scroll_Content_Obj;
        [SerializeField] private GameObject _Upgradeable_Entry_Panel_Prefab;
        [SerializeField] private List<UpgradeMaterialUI> _Upgradeable_Entry_Panel_Pool;

        // Refiner References
        [SerializeField] private GameObject _Refiner_Panel;
        [SerializeField] private UpgradeMaterialUI _Selected_Refining_Base_Block_Panel;
        [SerializeField] private TMP_Text _Refining_Labor_Still_Needed_Text;
        [SerializeField] private GameObject _Refiner_Scroll_Content_Obj;
        [SerializeField] private GameObject _Refiner_Recipe_Panel_Prefab;
        [SerializeField] private List<RefinerRecipeUI> _Refiner_Recipe_Panel_Pool;

        // Storage References
        [SerializeField] private GameObject _Storage_Panel;
        [SerializeField] private TMP_Text _Current_Storage_Text;
        [SerializeField] private TMP_Text _Stored_Block_Text;
        [SerializeField] private GameObject _Storage_Scroll_Content_Obj;
        [SerializeField] private GameObject _Stored_Item_Image_Prefab;
        [SerializeField] private List<Image> _Storage_Item_Panel_Pool;

        // Farm References
        [SerializeField] private GameObject _Farm_Panel;
        [SerializeField] private TMP_Text _Farm_Type_Text;
        [SerializeField] private TMP_Text _Farm_Needed_Labor_Text;
        [SerializeField] private UpgradeMaterialUI _Farm_Secondary_Resource_Panel;
        [SerializeField] private TMP_Text _Farm_Needed_Days_Text;
        [SerializeField] private GameObject _Farm_Slots_Scroll_Content_Obj;
        [SerializeField] private GameObject _Farm_Slot_Image_Prefab;
        [SerializeField] private List<Image> _Farm_Slot_Item_Panel_Pool;

        // Unlockable References
        [SerializeField] private GameObject _Unlockable_Panel;
        [SerializeField] private TMP_Text _Unlockable_Price_Text;

        // Button References
        [SerializeField] private GameObject _Button_Panel;
        [SerializeField] private GameObject _Sell_Button;
        [SerializeField] private GameObject _Generate_All_Button;
        [SerializeField] private GameObject _Upgrade_Button;


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

        public void ChangeBlockFocus(Block selected_Block)
        {
            if (current_Selected_Block == null)
            {
                return;
            }
            if (selected_Block.gameObject != current_Selected_Block.gameObject)
            {

                UpdateMenu(selected_Block);
            }
            else
            {
                Debug.Log("Deactivate");

                DeactivateMenu();
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
                case BlockType.Farm:
                    FarmSelection((Farm)selected_Block, (FarmData)block_Data);
                    break;
                case BlockType.Generator:
                    GeneratorSelection((Generator)selected_Block, (GeneratorData)block_Data);
                    break;
                case BlockType.House:
                    HouseSelection((House)selected_Block, (HouseData)block_Data);
                    break;
                case BlockType.Refiner:
                    RefinerSelection((Refiner)selected_Block, (RefinerData)block_Data);
                    break;
                case BlockType.Shop:
                    ShopSelection((Shop)selected_Block, (ShopData)block_Data);
                    break;
                case BlockType.Storage:
                    StorageSelection((Storage)selected_Block, (StorageData)block_Data);
                    break;
                case BlockType.Unlockable:
                    UnlockableSelection((Unlockable)selected_Block, (UnlockableData)block_Data);
                    break;
                case BlockType.Upgradeable:
                    UpgradeableSelection((Upgradeable)selected_Block, (UpgradeableData)block_Data);
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

            // Set the Selection Indicator on the current Blocks position
            if (selected_Block.block_Data.blockType != BlockType.Empty)
            {
                _Selection_Indicator.position = Camera.main.WorldToScreenPoint(selected_Block.transform.position);
            }

            Debug.Log("Activate");


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
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(blockData);

            // Activate useable Buttons
            _Sell_Button.SetActive(true);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
        }

        private void GeneratorSelection(Generator p_Generator, GeneratorData p_GeneratorData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(true);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(true);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_GeneratorData);

            // Charge
            _Days_charge_Text.text = "Days/Charge : " + p_GeneratorData.charge_Time;
            _Remaining_Text.text = p_Generator.remaining_Time + " Days remaining";
            _Stored_Text.text = p_Generator.item_Buffer.Count + " / " + p_GeneratorData.max_ItemBuffer + " Stored";

            // Results
            int result_Amount = p_GeneratorData.possible_Results.Count;
            int content_Window_Width = result_Amount * 130 + 40;
            _Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(content_Window_Width, 110);
            List<Image> images = _Scroll_Content_Obj
                .GetComponentsInChildren<Image>()
                .Take(result_Amount)
                .ToList();

            for (int i = 0; i < result_Amount; i++)
            {
                images[i].gameObject.SetActive(true);
                Sprite sprite = p_GeneratorData.possible_Results.list.ToList()[i].item.sprite;
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
            _Sell_Button.SetActive(p_GeneratorData.isSellable);
            // if it is a neighbor generator deactivate generate all button
            _Generate_All_Button.SetActive(!p_GeneratorData.isNeighborGenerator);
            _Upgrade_Button.SetActive(false);
        }

        private void WorkerSelection(Worker p_Worker, WorkerData p_Worker_Data)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(true);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(false);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_Worker_Data);

            // Worker Extra Info
            if (p_Worker.has_Home)
                _Worker_Has_Home_Image.sprite = true_Sprite;
            else
                _Worker_Has_Home_Image.sprite = false_Sprite;

            _Job_Text.text = "Job : " + p_Worker_Data.job.ToString();
            _Health_Text.text = p_Worker.currentHealth + " / " + p_Worker_Data.maxHP + " HP";
            _Damage_Text.text = p_Worker_Data.attackDamage + " Damage";
            _Defense_Text.text = p_Worker_Data.defense + " Defense";
            _Labor_Left_Text.text = p_Worker.current_Labor + " / " + p_Worker_Data.max_Labor + " Labor";



            // Activate useable Buttons
            _Sell_Button.SetActive(p_Worker_Data.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
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
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(true);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_WorkstationData);

            // Workstation Info
            if (p_WorkstationData.isLimitless)
                _IsLimitless_Image.sprite = true_Sprite;
            else
                _IsLimitless_Image.sprite = false_Sprite;

            _ChargesLeft_Text.text = "Charges Left: " + p_Workstation.remainingCharges;
            _Needed_Labor_Text.text = p_Workstation.current_Needed_Labor + "/" + p_WorkstationData.needed_Labor + " Needed Labor until next Charge";
            _Items_Per_Charge_Text.text = p_WorkstationData.items_Per_Charge + " Items per Charge";
            _Storage_Text.text = p_Workstation.item_Buffer.Count + " Stored";



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
            _Sell_Button.SetActive(p_WorkstationData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
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
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_HouseData);

            // House Info
            _Room_Limit_Text.text = p_House.inhabitants.Count + " / " + p_HouseData.roomLimit + " Room used";

            // Inhabitants Scroll View
            List<Worker> l_Inhabitants_List = p_House.inhabitants;
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
                int l_Worker_Block_ID = l_Inhabitants_List[i].block_Data.id;
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

            // Activate useable Buttons
            _Sell_Button.SetActive(p_HouseData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
        }

        private void ShopSelection(Shop p_Shop, ShopData p_ShopData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(true);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(false);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_ShopData);

            // Shop Scroll View
            List<ShopEntry> l_Shop_Entry_List = p_ShopData.shopEntries;
            int l_Shop_Entries_Amount = l_Shop_Entry_List.Count;

            // Increase the ScrollView Contents height to fit all Entries
            int l_Inhabitants_Content_Window_Height = l_Shop_Entries_Amount * 120 + 20;
            _Shop_Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(520, l_Inhabitants_Content_Window_Height);

            // Increase the Shop UI Entries Pool if necessary
            while (_Shop_Entry_Panel_Pool.Count < l_Shop_Entries_Amount)
            {
                GameObject l_Shop_Entry_Object = Instantiate(_Shop_Entry_Panel_Prefab);
                l_Shop_Entry_Object.transform.SetParent(_Shop_Scroll_Content_Obj.transform);
                _Shop_Entry_Panel_Pool.Add(l_Shop_Entry_Object.GetComponent<ShopUIEntry>());
            }

            for (int i = 0; i < l_Shop_Entries_Amount; i++)
            {
                _Shop_Entry_Panel_Pool[i].gameObject.SetActive(true);
                int l_Block_ID = l_Shop_Entry_List[i].BlockID;
                BlockData l_BlockData = _BlockManager.GetBlock_Data_By_ID(l_Block_ID);
                _Shop_Entry_Panel_Pool[i].SetUpEntry(l_BlockData.sprite, l_BlockData.name, l_Shop_Entry_List[i].Cost + " G", l_Block_ID);
            }
            for (int i = l_Shop_Entries_Amount; i < _Shop_Entry_Panel_Pool.Count; i++)
            {
                _Shop_Entry_Panel_Pool[i].gameObject.SetActive(false);
            }
        }

        private void UpgradeableSelection(Upgradeable p_Upgradeable, UpgradeableData p_UpgradeableData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(true);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_UpgradeableData);

            // Set Upgrade Target
            BlockData l_Target = p_UpgradeableData.upgrade_Target;
            _Upgrade_Target_Panel.SetUpMaterial(l_Target.sprite, l_Target.name);

            // Shop Scroll View
            List<UpgradeMaterial> l_Upgrade_Materials = p_Upgradeable._Upgrade_Materials;
            int l_Upgrade_Materials_Amount = l_Upgrade_Materials.Count;

            // Increase the ScrollView Contents height to fit all Entries
            int l_Upgrade_Content_Window_Height = l_Upgrade_Materials_Amount * 120 + 20;
            _Upgradeable_Scroll_Content_Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(520, l_Upgrade_Content_Window_Height);

            // Increase the Shop UI Entries Pool if necessary
            while (_Upgradeable_Entry_Panel_Pool.Count < l_Upgrade_Materials_Amount)
            {
                GameObject l_Upgrade_Material_Object = Instantiate(_Upgradeable_Entry_Panel_Prefab);
                l_Upgrade_Material_Object.transform.SetParent(_Upgradeable_Scroll_Content_Obj.transform);
                _Upgradeable_Entry_Panel_Pool.Add(l_Upgrade_Material_Object.GetComponent<UpgradeMaterialUI>());
            }

            for (int i = 0; i < l_Upgrade_Materials_Amount; i++)
            {
                _Upgradeable_Entry_Panel_Pool[i].gameObject.SetActive(true);
                BlockData l_Block = _BlockManager.GetBlock_Data_By_ID(l_Upgrade_Materials[i].block_ID);
                string l_Description_Text = l_Upgrade_Materials[i].has + " / " + l_Upgrade_Materials[i].needed + " " + l_Block.name;
                _Upgradeable_Entry_Panel_Pool[i].SetUpMaterial(l_Block.sprite, l_Description_Text);
            }
            for (int i = l_Upgrade_Materials_Amount; i < _Upgradeable_Entry_Panel_Pool.Count; i++)
            {
                _Upgradeable_Entry_Panel_Pool[i].gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(p_UpgradeableData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(true);
        }

        private void RefinerSelection(Refiner p_Refiner, RefinerData p_RefinerData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(true);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_RefinerData);

            // Set Selected Recipe
            if (p_Refiner._Is_A_Recipe_Selected)
            {
                RefiningRecipe l_Selected_Recipe = p_Refiner._Selected_Recipe;
                BlockData l_Base_Block = l_Selected_Recipe.base_Block_To_Refine;
                _Selected_Refining_Base_Block_Panel.SetUpMaterial(l_Base_Block.sprite, l_Base_Block.name);
                if (l_Selected_Recipe.is_Labor_Needed)
                {
                    _Refining_Labor_Still_Needed_Text.text = p_Refiner._Still_Needed_Labor + " Labor Still Needed";
                }
                else
                {
                    _Refining_Labor_Still_Needed_Text.text = "No Labor Needed";
                }
            }
            else
            {
                _Selected_Refining_Base_Block_Panel.SetUpMaterial(false_Sprite, "none Selected");
                _Refining_Labor_Still_Needed_Text.text = "No Labor Needed";
            }

            // Shop Scroll View
            List<RefiningRecipe> l_RefiningRecipes = p_RefinerData.refinement_Recipes;
            int l_RefiningRecipes_Amount = l_RefiningRecipes.Count;

            // Increase the Refining Recipes UI Entries Pool if necessary
            while (_Refiner_Recipe_Panel_Pool.Count < l_RefiningRecipes_Amount)
            {
                GameObject l_RefiningRecipesUI_Object = Instantiate(_Refiner_Recipe_Panel_Prefab);
                l_RefiningRecipesUI_Object.transform.SetParent(_Refiner_Scroll_Content_Obj.transform);
                _Refiner_Recipe_Panel_Pool.Add(l_RefiningRecipesUI_Object.GetComponent<RefinerRecipeUI>());
            }

            for (int i = 0; i < l_RefiningRecipes_Amount; i++)
            {
                _Refiner_Recipe_Panel_Pool[i].gameObject.SetActive(true);
                RefiningRecipe l_RefiningRecipe = l_RefiningRecipes[i];
                _Refiner_Recipe_Panel_Pool[i].SetUpBaseMaterial(l_RefiningRecipe.base_Block_To_Refine, l_RefiningRecipe.is_Labor_Needed, l_RefiningRecipe.needed_Labor);
                _Refiner_Recipe_Panel_Pool[i].SetUpResults(l_RefiningRecipe.refinement_Results);
            }
            for (int i = l_RefiningRecipes_Amount; i < _Refiner_Recipe_Panel_Pool.Count; i++)
            {
                _Refiner_Recipe_Panel_Pool[i].gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(p_RefinerData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
        }

        private void StorageSelection(Storage p_Storage, StorageData p_StorageData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(true);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_StorageData);

            // Storage Information
            _Current_Storage_Text.text = p_Storage.stored_Amount + "/" + p_StorageData.max_Storage;
            _Stored_Block_Text.text = _BlockManager.GetBlock_Data_By_ID(p_Storage.currently_Stored_Block_ID).name;


            // Storage Scroll View
            int l_Stored_Amount = p_Storage.stored_Amount;

            // Increase the Stored Item Images Pool if necessary
            while (_Storage_Item_Panel_Pool.Count < p_StorageData.max_Storage)
            {
                GameObject l_Stored_Item_Image_Object = Instantiate(_Stored_Item_Image_Prefab);
                l_Stored_Item_Image_Object.transform.SetParent(_Storage_Scroll_Content_Obj.transform);
                _Storage_Item_Panel_Pool.Add(l_Stored_Item_Image_Object.GetComponent<Image>());
            }

            // only stores one type of Block
            Sprite l_Stored_Item_Sprite = _BlockManager.GetBlock_Data_By_ID(p_Storage.currently_Stored_Block_ID).sprite;
            for (int i = 0; i < l_Stored_Amount; i++)
            {
                _Storage_Item_Panel_Pool[i].gameObject.SetActive(true);
                _Storage_Item_Panel_Pool[i].sprite = l_Stored_Item_Sprite;
            }
            for (int i = l_Stored_Amount; i < p_StorageData.max_Storage; i++)
            {
                _Storage_Item_Panel_Pool[i].gameObject.SetActive(true);
                _Storage_Item_Panel_Pool[i].sprite = false_Sprite;
            }
            for (int i = p_StorageData.max_Storage; i < _Storage_Item_Panel_Pool.Count; i++)
            {
                _Storage_Item_Panel_Pool[i].gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(p_StorageData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
        }

        private void FarmSelection(Farm p_Farm, FarmData p_FarmData)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(false);
            _Upgradeable_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(true);
            _Button_Panel.SetActive(true);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_FarmData);

            // Storage Information
            _Farm_Type_Text.text = p_FarmData.blockType.ToString();
            _Farm_Needed_Labor_Text.text = "Needs " + p_Farm._Still_Needed_Labor + " Labor";
            if (p_Farm._Has_SecondaryResource)
            {
                _Farm_Secondary_Resource_Panel.SetUpMaterial(true_Sprite, "Has enough " + p_FarmData.secondaryResource.name);
            }
            else
            {
                _Farm_Secondary_Resource_Panel.SetUpMaterial(p_FarmData.secondaryResource.sprite, "Needs " + p_FarmData.secondaryResource.name);
            }

            if (p_Farm._Still_Needed_Days <= 0)
            {
                _Farm_Needed_Days_Text.text = "Harvest Ready";
            }
            else if (p_Farm._Has_SecondaryResource && p_Farm._Still_Needed_Labor <= 0)
            {
                _Farm_Needed_Days_Text.text = "Growing... Needs " + p_Farm._Still_Needed_Days + " Days";
            }
            else
            {
                _Farm_Needed_Days_Text.text = "Not Ready to start Growing";
            }


            // Storage Scroll View
            int l_Stored_Amount = p_Farm._Slot_Entries.Count;

            // Increase the Stored Item Images Pool if necessary
            while (_Farm_Slot_Item_Panel_Pool.Count <= p_FarmData.max_Slots)
            {
                GameObject l_Slot_Image_Object = Instantiate(_Farm_Slot_Image_Prefab);
                l_Slot_Image_Object.transform.SetParent(_Farm_Slots_Scroll_Content_Obj.transform);
                _Farm_Slot_Item_Panel_Pool.Add(l_Slot_Image_Object.GetComponent<Image>());
            }


            for (int i = 0; i < l_Stored_Amount; i++)
            {
                _Farm_Slot_Item_Panel_Pool[i].gameObject.SetActive(true);
                Sprite l_Stored_Item_Sprite = _BlockManager.GetBlock_Data_By_ID(p_Farm._Slot_Entries[i]).sprite;
                _Farm_Slot_Item_Panel_Pool[i].sprite = l_Stored_Item_Sprite;
            }
            for (int i = l_Stored_Amount; i < p_FarmData.max_Slots; i++)
            {
                _Farm_Slot_Item_Panel_Pool[i].gameObject.SetActive(true);
                _Farm_Slot_Item_Panel_Pool[i].sprite = false_Sprite;
            }
            for (int i = p_FarmData.max_Slots; i < _Farm_Slot_Item_Panel_Pool.Count; i++)
            {
                _Farm_Slot_Item_Panel_Pool[i].gameObject.SetActive(false);
            }

            // Activate useable Buttons
            _Sell_Button.SetActive(p_FarmData.isSellable);
            _Generate_All_Button.SetActive(false);
            _Upgrade_Button.SetActive(false);
        }

        private void UnlockableSelection(Unlockable p_Unlockable, UnlockableData p_Unlockable_Data)
        {
            // Set needed Panels active and others to inactive
            _Header_Panel.SetActive(true);
            _Description_Panel.SetActive(true);
            _Charge_Panel.SetActive(false);
            _House_Info_Panel.SetActive(false);
            _Worker_Extra_Info_Panel.SetActive(false);
            _Workstation_Info_Panel.SetActive(false);
            _Refiner_Panel.SetActive(false);
            _Results_Panel.SetActive(false);
            _Shop_Panel.SetActive(false);
            _Storage_Panel.SetActive(false);
            _Farm_Panel.SetActive(false);
            _Unlockable_Panel.SetActive(true);
            _Upgradeable_Panel.SetActive(false);
            _Button_Panel.SetActive(false);

            // Fill in the Data

            // Header and Description
            AddBasicBlockInfo(p_Unlockable_Data);

            // Worker Extra Info
            _Unlockable_Price_Text.text = p_Unlockable_Data.price + "G";
        }

        public void AddBasicBlockInfo(BlockData p_Block_Data)
        {
            // Header
            _Main_Image.sprite = p_Block_Data.sprite;
            _Name_Text.text = p_Block_Data.name;
            _Level_Text.text = p_Block_Data.blockType.ToString() + "\nLvl " + p_Block_Data.level;
            _Max_Tag.SetActive(p_Block_Data.isMaxLevel);
            if (p_Block_Data.isSellable)
            {
                _Value_Text.alpha = 1;
                _Value_Text.text = p_Block_Data.value + "|G";
            }
            else
            {
                _Value_Text.alpha = 0;
            }

            // Description
            _Description_Text.text = p_Block_Data.description;
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

        public void Upgrade()
        {
            ((Upgradeable)current_Selected_Block).UpgradeBlock();
        }

        public void UnlockBlock()
        {
            game_Manager.UnlockBlock((Unlockable)current_Selected_Block);
        }

        public void BuyBlock(int p_BlockID)
        {
            int l_New_Player_Gold = ((Shop)current_Selected_Block).BuyBlock(p_BlockID, game_Manager.player_Data.gold);
            game_Manager.player_Data.gold = l_New_Player_Gold;
        }
    }
}
