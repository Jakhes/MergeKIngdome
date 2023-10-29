using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.UI
{
    [CreateAssetMenu(menuName = "UiElement/PopUpData")]
    public class PopUpData : ScriptableObject
    {
        [SerializeField] private string _Title;

        [TextArea]
        [SerializeField] private string _Main_Text;
        [SerializeField] private string _Button_Text;
        [SerializeField] private Color _Rim_Color;


        public string Title { get => _Title; }
        public string Main_Text { get => _Main_Text; }
        public string Button_Text { get => _Button_Text; }
        public Color Rim_Color { get => _Rim_Color; }
    }
}

