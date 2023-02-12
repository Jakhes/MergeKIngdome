using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class BlockInteraction : MonoBehaviour
    {
        [SerializeField] public Block block;
        [SerializeField] private float merge_Distance_Threshold = 0.5f;
        //TODO: change to a ref from BoardManager
        [SerializeField] public Board current_Board;
        [SerializeField] private List<Block> colliding_Blocks = new List<Block>();
        public bool isPickedUp = false;

        private void OnMouseUp()
        {
            if (block.IsMoveable && !PauseMenu.isGamePaused)
            {
                Block merge_Block = FindClosestTouchingBlock(colliding_Blocks);
                float distance = Vector2.Distance(this.transform.position, block.Parent_Node.transform.position);
                if (merge_Block != null && distance > merge_Distance_Threshold)
                {
                    current_Board.TryMergingBlocks(block, merge_Block);
                }
                else
                {
                    block.MoveBlockToNode();
                }
                colliding_Blocks.Clear();
                isPickedUp = false;
                block.Block_image.sortingLayerName = "Default";
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (isPickedUp)
            {
                Block colliding_Block;
                if (other.gameObject.TryGetComponent<Block>(out colliding_Block))
                {
                    colliding_Blocks.Add(colliding_Block);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (isPickedUp)
            {
                Block colliding_Block;
                if (other.gameObject.TryGetComponent<Block>(out colliding_Block))
                {
                    colliding_Blocks.Remove(colliding_Block);
                }
            }
        }

        private Block FindClosestTouchingBlock(List<Block> touching_Blocks)
        {
            float min_Distance = 10000f;
            Block merge_Block = null;
            foreach (var block in touching_Blocks)
            {
                float distance = Vector2.Distance(this.transform.position, block.transform.position);
                if (distance < min_Distance)
                {
                    merge_Block = block;
                    min_Distance = distance;
                }
            }
            return merge_Block;
        }
    }
}
