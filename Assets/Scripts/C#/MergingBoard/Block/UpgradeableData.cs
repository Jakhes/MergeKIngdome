using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Upgradeable", menuName = "BlockData/Upgradeable", order = 0)]
    public class UpgradeableData : BlockData
    {
        [SerializeField] public List<UpgradeMaterial> initial_Upgrade_Materials;
        [SerializeField] public BlockData upgrade_Target;
        [SerializeField] public bool is_Castle_Upgrade;
    }
}
