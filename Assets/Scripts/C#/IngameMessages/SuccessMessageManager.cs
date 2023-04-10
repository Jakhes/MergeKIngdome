using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace EvolvingCode.IngameMessages
{
    public class SuccessMessageManager : MonoBehaviour
    {
        [SerializeField] private Transform _Canvas;
        [SerializeField] private FloatingSuccessMessage _Floating_SuccessMessage_Prefab;

        private IObjectPool<FloatingSuccessMessage> _Message_Object_Pool;

        private void Awake()
        {
            _Message_Object_Pool = new ObjectPool<FloatingSuccessMessage>(OnCreatePoolObject, OnTakePoolObject, OnReturnPoolObject, OnDestroyPoolObject);
        }

        private FloatingSuccessMessage OnCreatePoolObject()
        {
            FloatingSuccessMessage l_Message = Instantiate(_Floating_SuccessMessage_Prefab);
            l_Message.pool = _Message_Object_Pool;
            l_Message.gameObject.transform.SetParent(_Canvas);

            return l_Message;
        }

        private void OnTakePoolObject(FloatingSuccessMessage p_Message)
        {
            p_Message.gameObject.SetActive(true);
        }

        private void OnReturnPoolObject(FloatingSuccessMessage p_Message)
        {
            p_Message.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(FloatingSuccessMessage p_Message)
        {
            Destroy(p_Message.gameObject);
        }




        //
        public void Sold(Vector3 p_Block_Position)
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Sold");
            l_Message.PlayAnimation();
        }

        //
        public void Bought()
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Bought");
            l_Message.PlayAnimation();
        }

        //
        public void Harvesting()
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Input.mousePosition;
            l_Message.ChangeText("Harvesting");
            l_Message.PlayAnimation();
        }

        //
        public void Upgrade(Vector3 p_Block_Position)
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Upgrade");
            l_Message.PlayAnimation();
        }

        //
        public void WorkstationChargeDone(Vector3 p_Block_Position)
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Done");
            l_Message.PlayAnimation();
        }

        internal void Refining(Vector3 p_Block_Position)
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Refining");
            l_Message.PlayAnimation();
        }

        internal void Generating(Vector3 p_Block_Position)
        {
            FloatingSuccessMessage l_Message = _Message_Object_Pool.Get();
            l_Message.gameObject.transform.position = Camera.main.WorldToScreenPoint(p_Block_Position);
            l_Message.ChangeText("Generating");
            l_Message.PlayAnimation();
        }
    }
}
