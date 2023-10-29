using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace EvolvingCode.UI
{
    public class PopUpController : MonoBehaviour
    {
        [SerializeField] private PopUpWindow _PopUpWindow;

        [SerializedDictionary("id", "Popup Data")]
        public SerializedDictionary<string, PopUpData> _PopUpData_List;


        void Update()
        {
            if (Input.GetMouseButton(2))
            {
                Activate_PopUpWindow("new");
            }
        }

        public void Activate_PopUpWindow(string p_Key)
        {
            if (!_PopUpData_List.ContainsKey(p_Key))
            {
                return;
            }

            PopUpData selected_PopUp = _PopUpData_List[p_Key];
            _PopUpWindow.Activate(selected_PopUp);
        }
    }
}