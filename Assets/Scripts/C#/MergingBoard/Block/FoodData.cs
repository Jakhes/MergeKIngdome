using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Food", menuName = "BlockData/Food", order = 0)]
    public class FoodData : BlockData
    {
        [SerializeField] public int maxCharges;
    }
}
