using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class Upgradeable : Block
    {
        [SerializeField] private List<UpgradeMaterial> _Upgrade_Materials;
        [SerializeField] private bool _Is_Upgrade_Ready;


        void Update()
        {
            UpgradeBlock();
        }

        public bool TryTakeBlock(Block p_Target_Block)
        {
            // return true if Block is part of the _Upgrade_Materials and is still needed
            for (int i = 0; i < _Upgrade_Materials.Count; i++)
            {
                UpgradeMaterial l_Selected_Material = _Upgrade_Materials[i];
                // Look if the Target Block is needed in the Materials List and if it has less than needed
                if (l_Selected_Material.block == p_Target_Block.block_Data && l_Selected_Material.has < l_Selected_Material.needed)
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

            foreach (var item in init_Block_Data.initial_Upgrade_Materials)
            {
                _Upgrade_Materials.Add(item);
            }
            _Is_Upgrade_Ready = false;
        }

        public void init_Block(UpgradeableData init_Block_Data, float p_Travel_Time, Upgradeable_Save_Data block_Save)
        {
            init_Block((BlockData)init_Block_Data, p_Travel_Time);

            foreach (var item in block_Save.saved_Materials)
            {
                _Upgrade_Materials.Add(item);
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
        public BlockData block;
        public int needed;
        public int has;

        public UpgradeMaterial(BlockData p_Block, int p_Needed, int p_Has)
        {
            block = p_Block;
            needed = p_Needed;
            has = p_Has;
        }
    }
}
