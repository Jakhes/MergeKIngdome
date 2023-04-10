using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace EvolvingCode.IngameMessages
{
    public class InfoMessageManager : MonoBehaviour
    {
        [SerializeField] private Transform _Canvas;
        [SerializeField] private FloatingInfoMessage _Floating_InfoMessage_Prefab;

        private IObjectPool<FloatingInfoMessage> _Message_Object_Pool;

        private void Awake()
        {
            _Message_Object_Pool = new ObjectPool<FloatingInfoMessage>(OnCreatePoolObject, OnTakePoolObject, OnReturnPoolObject, OnDestroyPoolObject);
        }

        private FloatingInfoMessage OnCreatePoolObject()
        {
            FloatingInfoMessage l_Message = Instantiate(_Floating_InfoMessage_Prefab);
            l_Message.pool = _Message_Object_Pool;
            l_Message.gameObject.transform.SetParent(_Canvas);

            return l_Message;
        }

        private void OnTakePoolObject(FloatingInfoMessage p_Message)
        {
            p_Message.gameObject.SetActive(true);
        }

        private void OnReturnPoolObject(FloatingInfoMessage p_Message)
        {
            p_Message.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(FloatingInfoMessage p_Message)
        {
            Destroy(p_Message.gameObject);
        }


        public void ClickMessage()
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.PlayAnimation();
        }

        // Message for notifying when a Worker was assigned to a House
        public void AssignHouse()
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("House Assigned");
            l_Message.PlayAnimation();
        }

        // Displays the amount of Labor gets recharged on nextDay
        public void LaborRecharged(int p_Max_Labor, Vector3 p_Worker_Position)
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            // using main might not be proficient
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Worker_Position);
            l_Message.ChangeText("+" + p_Max_Labor + " Labor");
            l_Message.PlayAnimation();
        }

        //
        public void LaborUsed(int p_Needed_Labor, int p_Max_Labor, Vector3 p_Workstation_Position)
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            // using main might not be proficient
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Workstation_Position);
            l_Message.ChangeText("(" + p_Needed_Labor + "/" + p_Max_Labor + ") Labor");
            l_Message.PlayAnimation();
        }

        //
        internal void Growing(int still_Needed_Days, Vector3 p_Farm_Position)
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            // using main might not be proficient
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Farm_Position);
            l_Message.ChangeText("Growing " + still_Needed_Days);
            l_Message.PlayAnimation();
        }

        //
        internal void NoChargesLeft(Vector3 p_Block_Position)
        {
            FloatingInfoMessage l_Message = _Message_Object_Pool.Get();
            // using main might not be proficient
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("No Charges Left");
            l_Message.PlayAnimation();
        }
    }
}
