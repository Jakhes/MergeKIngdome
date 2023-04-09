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
    }
}
