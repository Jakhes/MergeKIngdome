using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Upgradeable : Block
    {
        [SerializeField] public List<UpgradeMaterial> _Upgrade_Materials;
        [SerializeField] private bool _Is_Upgrade_Ready;




        public bool TryTakeBlock(Block p_Target_Block)
        {
            // return true if Block is part of the _Upgrade_Materials and is still needed
            for (int i = 0; i < _Upgrade_Materials.Count; i++)
            {
                UpgradeMaterial l_Selected_Material = _Upgrade_Materials[i];
                // Look if the Target Block is needed in the Materials List and if it has less than needed
                if (l_Selected_Material.block_ID == p_Target_Block.block_Data.id && l_Selected_Material.has < l_Selected_Material.needed)
                {
                    l_Selected_Material.has += 1;
                    _Upgrade_Materials[i] = l_Selected_Material;
                    _Is_Upgrade_Ready = AreAllMaterialsGathered();
                    return true;
                }
            }
            // else return false
            return false;
        }

        private bool AreAllMaterialsGathered()
        {
            foreach (UpgradeMaterial l_Material in _Upgrade_Materials)
            {
                if (l_Material.has != l_Material.needed)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpgradeBlock()
        {
            if (_Is_Upgrade_Ready)
            {
                Board parent_Board = this.GetComponentInParent<Board>();
                // this destroys this Block
                parent_Board.ReplaceBlock(this, ((UpgradeableData)block_Data).upgrade_Target.id);
            }
        }

        public void init_Block(UpgradeableData init_Block_Data, float p_Travel_Time)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            foreach (UpgradeMaterial item in init_Block_Data.initial_Upgrade_Materials)
            {
                _Upgrade_Materials.Add(item);
            }
            _Is_Upgrade_Ready = false;
        }

        public void init_Block(UpgradeableData init_Block_Data, float p_Travel_Time, Upgradeable_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            _Upgrade_Materials = new List<UpgradeMaterial>();
            foreach (UpgradeMaterial item in init_Block_Data.initial_Upgrade_Materials)
            {
                _Upgrade_Materials.Add(item);
            }

            foreach (UpgradeMaterial item in block_Save.saved_Materials)
            {
                int l_Index = _Upgrade_Materials.FindIndex(x => x.block_ID == item.block_ID);
                if (l_Index >= 0)
                {
                    UpgradeMaterial l_Updated_Material = _Upgrade_Materials[l_Index];
                    l_Updated_Material.has = Mathf.Clamp(item.has, 0, _Upgrade_Materials[l_Index].needed);
                    _Upgrade_Materials[l_Index] = l_Updated_Material;
                }
            }
            _Is_Upgrade_Ready = block_Save.is_Upgrade_Ready;
        }

        public new Upgradeable_Save_Data SaveBlock()
        {
            Upgradeable_Save_Data upgradeable_Save = new Upgradeable_Save_Data(base.SaveBlock(), _Upgrade_Materials, _Is_Upgrade_Ready);
            return upgradeable_Save;
        }
    }

    [System.Serializable]
    public struct UpgradeMaterial
    {
        // there are Problems when saving Block Data persistently so i use the id instead
        public int block_ID;
        public int needed;
        public int has;

        public UpgradeMaterial(int p_Block_ID, int p_Needed, int p_Has)
        {
            block_ID = p_Block_ID;
            needed = p_Needed;
            has = p_Has;
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
