using System;
using System.Collections.Generic;
using System.Linq;
using EvolvingCode.IngameMessages;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Farm : Block
    {
        [SerializeField] private FarmingResultsManager _FarmingResultsManager;
        [SerializeField] public List<int> _Slot_Entries;
        [SerializeField] public bool _Has_SecondaryResource;
        [SerializeField] public int _Still_Needed_Labor;
        [SerializeField] public int _Still_Needed_Days;
        [SerializeField] private List<int> _Item_Buffer;

        private void Start()
        {
            _FarmingResultsManager = FindObjectOfType<FarmingResultsManager>();
        }

        private void Update()
        {
            if (_Item_Buffer.Count > 0)
            {
                TryEmptyStorage();
            }
        }


        public bool TryAddingSlotItem(int p_Potential_Slot_Block_ID)
        {
            bool l_Is_Valid = _FarmingResultsManager.CheckIfValidSlotItem(p_Potential_Slot_Block_ID, ((FarmData)block_Data).farmType);

            if (l_Is_Valid && _Slot_Entries.Count < ((FarmData)block_Data).max_Slots)
            {
                _Slot_Entries.Add(p_Potential_Slot_Block_ID);
                _Still_Needed_Days = ((FarmData)block_Data).needed_Days;

                return true;
            }
            WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
            l_WarningMessageManager.Invalid(transform.position);
            return false;
        }

        public bool TryAddingSecondaryResource(int p_Potential_Resource_Block_ID)
        {

            if (((FarmData)block_Data).needs_SecondaryResource &&
                ((FarmData)block_Data).secondaryResource.id == p_Potential_Resource_Block_ID &&
                !_Has_SecondaryResource)
            {
                _Has_SecondaryResource = true;

                return true;
            }
            WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
            l_WarningMessageManager.Invalid(transform.position);
            return false;
        }

        public bool TryWorkOnFarm(Worker p_Worker)
        {
            if (((FarmData)block_Data).needs_Labor && _Still_Needed_Labor > 0)
            {
                _Still_Needed_Labor = p_Worker.UseLabor(_Still_Needed_Labor);
            }
            else
            {
                WarningMessageManager l_WarningMessageManager = this.GetComponentInParent<Board>().WarningMessageManager;
                l_WarningMessageManager.Invalid(transform.position);
            }

            return false;
        }

        private void TryHarvesting()
        {
            if (_Slot_Entries.Count > 0 && _Item_Buffer.Count <= 0 && _Still_Needed_Days <= 0)
            {
                List<int> l_Result_IDs = _FarmingResultsManager.GetFarmingResultsForIDs(_Slot_Entries, ((FarmData)block_Data).farmType);
                foreach (int l_Result_ID in l_Result_IDs)
                {
                    _Item_Buffer.Add(l_Result_ID);
                }

                _Has_SecondaryResource = false;
                _Still_Needed_Labor = ((FarmData)block_Data).needed_Labor;
                _Still_Needed_Days = ((FarmData)block_Data).needed_Days;
                _Slot_Entries.RemoveAll(x => true);

                SuccessMessageManager l_SuccessMessageManager = this.GetComponentInParent<Board>().SuccessMessageManager;
                l_SuccessMessageManager.Harvesting();
            }
        }

        public void NextDay()
        {
            if (_Slot_Entries.Count > 0 &&
                _Still_Needed_Days > 0 &&
                (!((FarmData)block_Data).needs_SecondaryResource || _Has_SecondaryResource) &&
                (!((FarmData)block_Data).needs_Labor || _Still_Needed_Labor <= 0))
            {
                _Still_Needed_Days -= 1;
                InfoMessageManager l_InfoMessageManager = this.GetComponentInParent<Board>().InfoMessageManager;
                l_InfoMessageManager.Growing(_Still_Needed_Days, transform.position);
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


        public void init_Block(FarmData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
            _Slot_Entries = new List<int>();
            _Has_SecondaryResource = false;
            _Still_Needed_Labor = init_Block_Data.needed_Labor;
            _Still_Needed_Days = init_Block_Data.needed_Days;
            _Item_Buffer = new List<int>();
        }

        internal void init_Block(FarmData init_Block_Data, float p_Travel_Time, Farm_Save_Data save_Data)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);
            _Slot_Entries = save_Data._Slot_Entries;
            _Has_SecondaryResource = save_Data._Has_SecondaryResource;
            _Still_Needed_Labor = save_Data._Still_Needed_Labor;
            _Still_Needed_Days = save_Data._Still_Needed_Days;
            _Item_Buffer = save_Data._Item_Buffer;
        }

        public new Farm_Save_Data SaveBlock()
        {
            Farm_Save_Data Farm_Save = new Farm_Save_Data(
                base.SaveBlock(), _Slot_Entries, _Has_SecondaryResource, _Still_Needed_Labor, _Still_Needed_Days, _Item_Buffer);

            return Farm_Save;
        }

        public override void DoubleClickAction()
        {
            Debug.Log("Click");
            TryHarvesting();
        }
    }

    public enum FarmType
    {
        CropFarm,
        AnimalFarm,
        MonsterFarm
    }



    [System.Serializable]
    public struct Farm_Save_Data
    {
        public Block_Save_Data base_Block_Save;
        public List<int> _Slot_Entries;
        public bool _Has_SecondaryResource;
        public int _Still_Needed_Labor;
        public int _Still_Needed_Days;
        public List<int> _Item_Buffer;

        public Farm_Save_Data(
            Block_Save_Data p_Base_Block_Save,
            List<int> _p_Slot_Entries,
            bool p_Has_SecondaryResource,
            int p_Still_Needed_Labor,
            int p_Still_Needed_Days,
            List<int> p_Item_Buffer)
        {
            base_Block_Save = p_Base_Block_Save;
            _Slot_Entries = _p_Slot_Entries;
            _Has_SecondaryResource = p_Has_SecondaryResource;
            _Still_Needed_Labor = p_Still_Needed_Labor;
            _Still_Needed_Days = p_Still_Needed_Days;
            _Item_Buffer = p_Item_Buffer;
        }
    }


}