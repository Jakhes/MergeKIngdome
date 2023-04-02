using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace EvolvingCode.MergingBoard
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Node parent_Node;
        [SerializeField] protected float travel_Time;
        [SerializeField] public BlockData block_Data;

        [SerializeField] private SpriteRenderer block_image;

        public bool IsMergeable { get => block_Data.isMergeable; set => block_Data.isMergeable = value; }
        public bool IsMoveable { get => block_Data.isMoveable; set => block_Data.isMoveable = value; }
        public bool IsEmpty { get => block_Data.blockType == BlockType.Empty; }
        public Node Parent_Node { get => parent_Node; set => parent_Node = value; }
        public BlockType BlockType { get => block_Data.blockType; }
        public SpriteRenderer Block_image { get => block_image; set => block_image = value; }


        public void MoveBlockToNode()
        {
            transform.DOMove(Parent_Node.transform.position, travel_Time);
        }

        public void init_Block(BlockData init_Block_Data, float p_Travel_Time)
        {
            block_Data = init_Block_Data;
            travel_Time = p_Travel_Time;
            //Text.text = block_Data.name;
            Block_image.sprite = block_Data.sprite;
        }

        public void init_Block(BlockData init_Block_Data, float p_Travel_Time, Block_Save_Data block_Save)
        {
            init_Block(init_Block_Data, p_Travel_Time);
        }

        public virtual Block_Save_Data SaveBlock()
        {
            Block_Save_Data block_Save = new Block_Save_Data(block_Data.id, Parent_Node.board_Pos);
            return block_Save;
        }

        public virtual void DoubleClickAction()
        {

        }

        public virtual void SellBlock()
        {

        }
    }

    [System.Serializable]
    public struct Block_Save_Data
    {
        public int id;
        public Vector2 node_Pos;

        public Block_Save_Data(int p_ID, Vector2 p_Node_Pos)
        {
            id = p_ID;
            node_Pos = p_Node_Pos;
        }
    }
}
