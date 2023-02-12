using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Refiner", menuName = "BlockData/Refiner", order = 0)]
    public class RefinerData : BlockData
    {
        public List<RefiningRecipe> refinement_Recipes;
    }
}
