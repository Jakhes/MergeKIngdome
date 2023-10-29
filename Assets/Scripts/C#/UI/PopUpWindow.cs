using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EvolvingCode.UI
{
    public class PopUpWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _Title_Text;
        [SerializeField] private TMP_Text _Main_Text;
        [SerializeField] private TMP_Text _Button_Text;
        [SerializeField] private Image _Rim_Image;


        public void Activate(PopUpData p_PopUpData)
        {
            _Title_Text.text = p_PopUpData.Title;
            _Main_Text.text = p_PopUpData.Main_Text;
            _Button_Text.text = p_PopUpData.Button_Text;
            _Rim_Image.color = p_PopUpData.Rim_Color;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}