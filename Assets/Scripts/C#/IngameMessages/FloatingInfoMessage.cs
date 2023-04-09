using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Pool;

namespace EvolvingCode.IngameMessages
{
    public class FloatingInfoMessage : MonoBehaviour
    {
        [SerializeField] private float _LifeTime;
        [SerializeField] private float _Current_Time = 0;

        [SerializeField] private Animator _Text_Animation;

        public IObjectPool<FloatingInfoMessage> pool;

        void Update()
        {
            _Current_Time += Time.deltaTime;
            if (_Current_Time > _LifeTime)
            {
                pool.Release(this);
            }
        }
        public void PlayAnimation()
        {
            _Current_Time = 0;
            _Text_Animation.Play("FloatingText");
        }
    }
}
