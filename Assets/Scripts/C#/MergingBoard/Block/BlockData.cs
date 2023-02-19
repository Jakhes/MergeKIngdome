using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
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
        [SerializeField] public int value;
    }
}
