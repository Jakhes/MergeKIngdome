using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace EvolvingCode
{
    public class ShopUIEntry : MonoBehaviour
    {
        [SerializeField] private int _BlockID;
        [SerializeField] private Image _Block_Image;
        [SerializeField] private TMP_Text _Cost_Text;

        public void BuyBlock()
        {
            GetComponentInParent<BlockInfoMenuController>().BuyBlock(_BlockID);
        }

        public void SetUpEntry(Sprite p_Block_Image, string p_Cost_String, int p_Block_ID)
        {
            _Block_Image.sprite = p_Block_Image;
            _Cost_Text.text = p_Cost_String;
            _BlockID = p_Block_ID;
        }
    }
}
