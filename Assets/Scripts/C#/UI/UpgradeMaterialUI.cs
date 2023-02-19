using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace EvolvingCode
{
    public class UpgradeMaterialUI : MonoBehaviour
    {
        [SerializeField] private Image _Block_Image;
        [SerializeField] private TMP_Text _Description_Text;

        public void SetUpMaterial(Sprite p_Block_Image, string p_Description)
        {
            _Block_Image.sprite = p_Block_Image;
            _Description_Text.text = p_Description;
        }
    }
}
