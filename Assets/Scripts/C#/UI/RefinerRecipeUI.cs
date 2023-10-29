using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using EvolvingCode.MergingBoard;

namespace EvolvingCode.UI
{
    public class RefinerRecipeUI : MonoBehaviour
    {
        [SerializeField] private UpgradeMaterialUI _Base_Block;
        [SerializeField] private TMP_Text _Needed_Labor;
        [SerializeField] private GameObject _UpgradeMaterialUI_Prefab;
        [SerializeField] private List<UpgradeMaterialUI> _Result_Blocks_Pool;

        public void SetUpBaseMaterial(BlockData p_Base, bool p_Is_Labor_Needed, int p_Needed_Labor)
        {
            _Base_Block.SetUpMaterial(p_Base.sprite, p_Base.name);
            if (p_Is_Labor_Needed)
            {
                _Needed_Labor.text = p_Needed_Labor + " Labor Needed";
            }
            else
            {
                _Needed_Labor.text = "No Labor Needed";
            }
        }

        public void SetUpResults(List<BlockData> p_Results)
        {
            int l_Results_Amount = p_Results.Count;
            // Increase the Shop UI Entries Pool if necessary
            while (_Result_Blocks_Pool.Count < l_Results_Amount)
            {
                GameObject l_Upgrade_Material_Object = Instantiate(_UpgradeMaterialUI_Prefab);
                l_Upgrade_Material_Object.transform.SetParent(gameObject.transform);
                _Result_Blocks_Pool.Add(l_Upgrade_Material_Object.GetComponent<UpgradeMaterialUI>());
            }
            for (int i = 0; i < l_Results_Amount; i++)
            {
                _Result_Blocks_Pool[i].gameObject.SetActive(true);
                BlockData l_Block = p_Results[i];
                string l_Description_Text = l_Block.name;
                _Result_Blocks_Pool[i].SetUpMaterial(l_Block.sprite, l_Description_Text);

            }
            for (int i = l_Results_Amount; i < _Result_Blocks_Pool.Count; i++)
            {
                _Result_Blocks_Pool[i].gameObject.SetActive(false);
            }
        }
    }
}
