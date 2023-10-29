using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace EvolvingCode.UI
{
    public class ShopUIEntry : MonoBehaviour
    {
        [SerializeField] private int _BlockID;
        [SerializeField] private Image _Block_Image;
        [SerializeField] private TMP_Text _Entry_Text;
        [SerializeField] private TMP_Text _Cost_Button_Text;

        public void BuyBlock()
        {
            GetComponentInParent<BlockInfoMenuController>().BuyBlock(_BlockID);
        }

        public void SetUpEntry(Sprite p_Block_Image, string p_Entry_String, string p_Cost_String, int p_Block_ID)
        {
            _Block_Image.sprite = p_Block_Image;
            _Entry_Text.text = p_Entry_String;
            _Cost_Button_Text.text = p_Cost_String;
            _BlockID = p_Block_ID;
        }
    }
}
