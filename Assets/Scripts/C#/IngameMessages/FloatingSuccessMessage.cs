using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace EvolvingCode.IngameMessages
{
    public class FloatingSuccessMessage : MonoBehaviour
    {
        [SerializeField] private float _LifeTime;
        [SerializeField] private float _Current_Time = 0;

        [SerializeField] private Animator _Text_Animation;
        [SerializeField] private TMP_Text _Message_Text;

        public IObjectPool<FloatingSuccessMessage> pool;

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

        public void ChangeText(string p_Text)
        {
            _Message_Text.text = p_Text;
        }
    }
}
