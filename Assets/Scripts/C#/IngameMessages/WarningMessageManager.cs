using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace EvolvingCode.IngameMessages
{
    public class WarningMessageManager : MonoBehaviour
    {
        [SerializeField] private Transform _Canvas;
        [SerializeField] private FloatingWarningMessage _Floating_WarningMessage_Prefab;

        private IObjectPool<FloatingWarningMessage> _Message_Object_Pool;

        private void Awake()
        {
            _Message_Object_Pool = new ObjectPool<FloatingWarningMessage>(OnCreatePoolObject, OnTakePoolObject, OnReturnPoolObject, OnDestroyPoolObject);
        }

        private FloatingWarningMessage OnCreatePoolObject()
        {
            FloatingWarningMessage l_Message = Instantiate(_Floating_WarningMessage_Prefab);
            l_Message.pool = _Message_Object_Pool;
            l_Message.gameObject.transform.SetParent(_Canvas);

            return l_Message;
        }

        private void OnTakePoolObject(FloatingWarningMessage p_Message)
        {
            p_Message.gameObject.SetActive(true);
        }

        private void OnReturnPoolObject(FloatingWarningMessage p_Message)
        {
            p_Message.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(FloatingWarningMessage p_Message)
        {
            Destroy(p_Message.gameObject);
        }




        //
        public void LostHouse(Vector3 p_Worker_Position)
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Worker_Position);
            l_Message.ChangeText("Lost Home!");
            l_Message.PlayAnimation();
        }

        //
        public void HouseFull()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Full!");
            l_Message.PlayAnimation();
        }

        //
        public void AlreadyAssigned()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("HasHome!");
            l_Message.PlayAnimation();
        }

        //
        public void NotEnoughLabor()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Not enough Labor!");
            l_Message.PlayAnimation();
        }

        //
        public void MissingResources()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Missing Resources!");
            l_Message.PlayAnimation();
        }

        //
        public void Invalid(Vector3 p_Block_Position)
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Invalid!");
            l_Message.PlayAnimation();
        }

        internal void BoardFull()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Board Full!");
            l_Message.PlayAnimation();
        }

        internal void NotEnoughGold(int p_Cost)
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Not enough, " + p_Cost + "!");
            l_Message.PlayAnimation();
        }

        internal void Full()
        {
            FloatingWarningMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Full!");
            l_Message.PlayAnimation();
        }
    }
}
