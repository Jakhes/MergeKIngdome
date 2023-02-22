using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    [CreateAssetMenu(fileName = "Block", menuName = "BlockData/Block", order = 0)]
    public class BlockData : ScriptableObject
    {
        [SerializeField] public int id;
        [SerializeField] public new string name;
        [SerializeField] public BlockType blockType;
        [SerializeField] public string description;
        [SerializeField] public Sprite sprite;

        [SerializeField] public bool isMoveable;
        [SerializeField] public bool isMergeable;

        [SerializeField] public bool isMaxLevel;
        [SerializeField] public int level;

        [SerializeField] public bool isSellable;
        [SerializeField] public int value;
    }
}
