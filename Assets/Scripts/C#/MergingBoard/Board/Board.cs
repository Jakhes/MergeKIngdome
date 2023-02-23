using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvolvingCode.MergingBoard
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private int board_Id;
        [SerializeField] private int width = 1;
        [SerializeField] private int height = 1;
        [SerializeField] private Node node_Prefab;

        [SerializeField] private Merger merge_Manager;
        [SerializeField] private BlockManager block_Manager;



        private List<Node> nodes;
        Vector2 grid_Position;
        Vector2 center;
        private bool isInitiated = false;
        public Vector2 offset;
        public float travel_Time;
        public float orthographicOffset;

        public BoardData saveBoard()
        {
            // Save all none empty Blocks
            List<Block_Save_Data> block_Saves = new List<Block_Save_Data>();
            List<Farm_Save_Data> farm_Saves = new List<Farm_Save_Data>();
            List<Food_Save_Data> food_Saves = new List<Food_Save_Data>();
            List<Generator_Save_Data> generator_Saves = new List<Generator_Save_Data>();
            List<House_Save_Data> house_Saves = new List<House_Save_Data>();
            List<Refiner_Save_Data> refiner_Saves = new List<Refiner_Save_Data>();
            List<Shop_Save_Data> shop_Saves = new List<Shop_Save_Data>();
            List<Upgradeable_Save_Data> upgradeable_Saves = new List<Upgradeable_Save_Data>();
            List<Worker_Save_Data> worker_Saves = new List<Worker_Save_Data>();
            List<WorkStation_Save_Data> workStation_Saves = new List<WorkStation_Save_Data>();

            nodes
                .Where(n => !n.current_Block.IsEmpty)
                .ToList()
                .ForEach(n =>
                    {
                        switch (n.current_Block.BlockType)
                        {
                            case BlockType.Resource:
                                block_Saves.Add(n.current_Block.SaveBlock());
                                break;
                            case BlockType.Farm:
                                farm_Saves.Add(((Farm)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Food:
                                food_Saves.Add(((Food)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Generator:
                                generator_Saves.Add(((Generator)n.current_Block).SaveBlock());
                                break;
                            case BlockType.House:
                                house_Saves.Add(((House)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Refiner:
                                refiner_Saves.Add(((Refiner)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Shop:
                                shop_Saves.Add(((Shop)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Upgradeable:
                                upgradeable_Saves.Add(((Upgradeable)n.current_Block).SaveBlock());
                                break;
                            case BlockType.Worker:
                                worker_Saves.Add(((Worker)n.current_Block).SaveBlock());
                                break;
                            case BlockType.WorkStation:
                                workStation_Saves.Add(((Workstation)n.current_Block).SaveBlock());
                                break;
                            default:
                                block_Saves.Add(n.current_Block.SaveBlock());
                                break;
                        }
                    });
            // Create BoardData
            BoardData boardData = new BoardData(width, height,
                block_Saves, farm_Saves, food_Saves, generator_Saves, house_Saves, refiner_Saves, shop_Saves, upgradeable_Saves, worker_Saves, workStation_Saves);
            return boardData;
        }



        public void loadBoard(BoardData boardData)
        {
            if (boardData == default(BoardData))
            {
                Debug.Log("No Board save found init normal Board!");
                InitiateBoard();
                return;
            }
            // Set Board Metrics
            width = (int)boardData.board_Dims.x;
            height = (int)boardData.board_Dims.y;
            if (isInitiated)
            {
                Debug.Log("Cleaning Board " + this.name);
                cleanBoard();
            }
            else
            {
                InitiateBoard();
            }
            // Load Blocks
            LoadBlockSaves(boardData.block_Saves);
            LoadBlockSaves(boardData.farm_Saves);
            LoadBlockSaves(boardData.food_Saves);
            LoadBlockSaves(boardData.generator_Saves);
            LoadBlockSaves(boardData.house_Saves);
            LoadBlockSaves(boardData.refiner_Saves);
            LoadBlockSaves(boardData.shop_Saves);
            LoadBlockSaves(boardData.upgradeable_Saves);
            LoadBlockSaves(boardData.worker_Saves);
            LoadBlockSaves(boardData.workStation_Saves);
        }

        public void cleanBoard()
        {
            List<Node> filledNodes = nodes
                .Where(n => !n.current_Block.IsEmpty)
                .ToList();
            foreach (var node in nodes)
            {
                var empty_Block = block_Manager.Create_Empty_Block();
                ReplaceBlock(node, empty_Block);
            }
        }

        // ? Can i make sure this gets called only once
        public void InitiateBoard()
        {
            isInitiated = true;
            grid_Position = this.transform.position;
            nodes = new List<Node>();
            // Calculate the Board center position
            center.x = (float)(width / 2 - (0.5 - (0.5 * (width % 2))));
            center.y = (float)(height / 2 - (0.5 - (0.5 * (height % 2))));

            // Create matrix of Nodes and fill them with empty Blocks
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var node = Instantiate(node_Prefab, grid_Position + new Vector2(x, y), Quaternion.identity);
                    node.transform.parent = this.transform;
                    node.board_Pos = new Vector2(x, y);

                    // Connect Node with Block and vise versa
                    Block empty_Block = block_Manager.Create_Empty_Block();
                    ReplaceBlock(node, empty_Block);

                    // Save Node ref into nodes List
                    nodes.Add(node);
                }
            }
            Vector3 vec = new Vector3(-center.x, -center.y, 0);
            transform.localPosition = vec;
        }

        public void FocusOnBoard()
        {
            // Set Main Camera position to the Board center
            Vector3 target_Pos = new Vector3(center.x, center.y, -10) + this.transform.position;
            Camera.main.transform.DOMove(target_Pos, travel_Time);
            // Adjust Camera size to fit Board into view

            float l_Target_OrthoSize = ((float)height + 1.0f) / 2.0f + 0.5f + ((float)height / 50) + orthographicOffset;
            Camera.main.GetComponent<Camera>().DOOrthoSize(l_Target_OrthoSize, travel_Time);
        }

        public async void FadeOutBoard()
        {
            await transform.parent.transform.DOScale(new Vector3(0.01f, 0.01f, 1), 1).AsyncWaitForCompletion();
            this.gameObject.SetActive(false);
        }

        public void UpdateCharges()
        {
            List<Node> generator_nodes = nodes
                .Where(n => n.current_Block.block_Data.blockType == BlockType.Generator)
                .ToList();
            foreach (var item in generator_nodes)
            {
                Generator generator = (Generator)item.current_Block;
                generator.Advance_Charge_Time();
            }
        }

        public virtual void NextDay()
        {
            List<Node> filledNodes = nodes
                .Where(n => !n.current_Block.IsEmpty)
                .ToList();
            foreach (var node in nodes)
            {
                if (node.current_Block.BlockType == BlockType.House)
                {
                    ((House)(node.current_Block)).NextDay();
                }
                if (node.current_Block.BlockType == BlockType.Farm)
                {
                    ((Farm)(node.current_Block)).NextDay();
                }
            }
        }

        //TODO: reuse Block so i don't have to Instantiate and Destroy
        public void SpawnBlocks(int amount, int id, List<Node> free_nodes)
        {
            foreach (var node in free_nodes.Take(amount))
            {
                var block = block_Manager.Create_Block(id);
                ReplaceBlock(node, block);
            }
        }

        public void SpawnBlocks(int amount, int id, Vector2 spawn_Pos, List<Node> free_nodes)
        {
            foreach (var node in free_nodes.Take(amount))
            {

                var block = block_Manager.Create_Block(id);
                if (block == null) return;
                ReplaceBlock(node, block);
                block.DOComplete();
                block.transform.position = spawn_Pos;
                block.MoveBlockToNode();
            }
        }

        public void RemoveBlock(Block block)
        {
            if (block != null)
            {
                var empty_Block = block_Manager.Create_Empty_Block();
                ReplaceBlock(block.Parent_Node, empty_Block);
            }
        }

        private void LoadBlockSaves(List<Block_Save_Data> block_Saves)
        {
            foreach (Block_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Farm_Save_Data> block_Saves)
        {
            foreach (Farm_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Food_Save_Data> block_Saves)
        {
            foreach (Food_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Generator_Save_Data> block_Saves)
        {
            foreach (Generator_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<House_Save_Data> block_Saves)
        {
            foreach (House_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Refiner_Save_Data> block_Saves)
        {
            foreach (Refiner_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Shop_Save_Data> block_Saves)
        {
            foreach (Shop_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Upgradeable_Save_Data> block_Saves)
        {
            foreach (Upgradeable_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<Worker_Save_Data> block_Saves)
        {
            foreach (Worker_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }
        private void LoadBlockSaves(List<WorkStation_Save_Data> block_Saves)
        {
            foreach (WorkStation_Save_Data blockSave in block_Saves)
            {
                Node parent_Node = nodes.First(n => n.board_Pos == blockSave.base_Block_Save.node_Pos);
                Block new_Block = block_Manager.Load_Block_From_Save(blockSave, parent_Node);
                new_Block.transform.parent = this.transform;
            }
        }


        public List<Node> GetNeighborNodes(Vector2 pos)
        {
            List<Node> neighbors = new List<Node>();

            if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) return neighbors;

            for (int direction = 0; direction < 9; direction++)
            {
                if (direction == 4) continue;
                int x_Dir = (int)pos.x + (direction % 3 - 1);
                int y_Dir = (int)pos.y + (direction / 3 - 1);

                if (x_Dir >= 0 && x_Dir < width && y_Dir >= 0 && y_Dir < height)
                {
                    Vector2 neighbor_Pos = new Vector2(x_Dir, y_Dir);
                    neighbors.Add(nodes.Find(n => n.board_Pos == neighbor_Pos));
                }
            }
            return neighbors;
        }

        public bool Try_Spawning_Block_On_Board(int id, Vector2 spawn_Pos)
        {
            var free_nodes = nodes
                                .Where(n => n.current_Block.IsEmpty)
                                .OrderBy(b => Random.value)
                                .ToList();
            if (free_nodes.Count > 0)
            {
                SpawnBlocks(1, id, spawn_Pos, free_nodes);
                return true;
            }
            return false;
        }

        public bool Try_Spawning_Worker_Block_On_Board_With_Save(int id, Vector2 spawn_Pos, Worker_Save_Data workerSave)
        {
            var free_nodes = nodes
                                .Where(n => n.current_Block.IsEmpty)
                                .OrderBy(b => Random.value)
                                .ToList();
            if (free_nodes.Count > 0)
            {
                Block old_Block = free_nodes[0].current_Block;
                var block = block_Manager.Load_Block_From_Save(workerSave, free_nodes[0]);
                if (block == null) return false;
                block.transform.parent = this.transform;
                block.transform.position = spawn_Pos;
                block.MoveBlockToNode();
                if (old_Block != null)
                {
                    Destroy(old_Block.gameObject);
                }
                return true;
            }
            return false;
        }

        public bool Try_Spawning_Food_Block_On_Board_With_Save(int id, Vector2 spawn_Pos, Food_Save_Data foodSave)
        {
            var free_nodes = nodes
                                .Where(n => n.current_Block.IsEmpty)
                                .OrderBy(b => Random.value)
                                .ToList();
            if (free_nodes.Count > 0)
            {
                Block old_Block = free_nodes[0].current_Block;
                var block = block_Manager.Load_Block_From_Save(foodSave, free_nodes[0]);
                if (block == null) return false;
                block.transform.parent = this.transform;
                block.transform.position = spawn_Pos;
                block.MoveBlockToNode();
                if (old_Block != null)
                {
                    Destroy(old_Block.gameObject);
                }
                return true;
            }
            return false;
        }

        public bool Try_Spawn_Block_In_Neighborhood(int id, Node spawner_Node)
        {
            List<Node> neighbor_Nodes = GetNeighborNodes(spawner_Node.board_Pos);
            var free_nodes = neighbor_Nodes
                                .Where(n => n.current_Block.IsEmpty)
                                .OrderBy(b => Random.value)
                                .ToList();
            if (free_nodes.Count > 0)
            {

                SpawnBlocks(1, id, spawner_Node.transform.position, free_nodes);
                return true;
            }
            return false;
        }

        public void TryMergingBlocks(Block A, Block B)
        {
            if (B.IsEmpty)
            {
                SwitchPlaceWithEmpty(A, B);
            }
            else if (A.BlockType == BlockType.Worker && B.BlockType == BlockType.WorkStation)
            {
                if (!((Workstation)B).TryTakeWorker((Worker)A))
                {
                    A.MoveBlockToNode();
                }
            }
            else if (A.BlockType == BlockType.Worker && B.BlockType == BlockType.House)
            {
                if (!((House)B).TryTakeWorker((Worker)A))
                {
                    A.MoveBlockToNode();
                }
            }
            else if (B.BlockType == BlockType.Upgradeable)
            {
                if (!((Upgradeable)B).TryTakeBlock(A))
                {
                    A.MoveBlockToNode();
                }
                else
                {
                    RemoveBlock(A);
                }
            }
            else if (A.BlockType == BlockType.Worker && B.BlockType == BlockType.Refiner)
            {
                if (!((Refiner)B).TryWorkOnRefinement((Worker)A))
                {
                    A.MoveBlockToNode();
                }
                else
                {
                    A.MoveBlockToNode();
                }
            }
            else if (B.BlockType == BlockType.Refiner)
            {
                if (!((Refiner)B).TrySelectingRecipe(A.block_Data.id))
                {
                    A.MoveBlockToNode();
                }
                else
                {
                    RemoveBlock(A);
                }
            }
            else if (A.BlockType == BlockType.Worker && B.BlockType == BlockType.Farm)
            {
                if (!((Farm)B).TryWorkOnFarm((Worker)A))
                {
                    A.MoveBlockToNode();
                }
                else
                {
                    A.MoveBlockToNode();
                }
            }
            else if (B.BlockType == BlockType.Farm)
            {
                if (((Farm)B).TryAddingSlotItem(A.block_Data.id))
                {
                    RemoveBlock(A);
                }
                else if (((Farm)B).TryAddingSecondaryResource(A.block_Data.id))
                {
                    RemoveBlock(A);
                }
                else
                {

                    A.MoveBlockToNode();
                }
            }
            else
            {
                int result_Block_ID;
                if (merge_Manager.is_Valid_Merge(A.block_Data.id, B.block_Data.id, out result_Block_ID))
                {
                    MergeBlocks(A, B, result_Block_ID);
                }
                else
                {
                    PushOtherBlockAway(A, B);
                }
            }
        }

        public void ReplaceBlock(Node p_To_Be_Replaced, int p_Replacer_Id)
        {
            var l_Replacer_Block = block_Manager.Create_Block(p_Replacer_Id);
            ReplaceBlock(p_To_Be_Replaced, l_Replacer_Block);
        }

        // Takes the Node of the Block_To_Be_Replaced and Replaces the Block with the given Replacer
        public void ReplaceBlock(Node p_To_Be_Replaced, Block p_Replacer_Block)
        {
            // Remove old Block
            Block l_Old_Block = p_To_Be_Replaced.current_Block;
            if (l_Old_Block != null)
                Destroy(l_Old_Block.gameObject);

            // Fill in the replacer Block
            p_To_Be_Replaced.current_Block = p_Replacer_Block;
            p_Replacer_Block.Parent_Node = p_To_Be_Replaced;
            p_Replacer_Block.transform.parent = this.transform;
            p_Replacer_Block.transform.position = p_To_Be_Replaced.transform.position;
        }

        private void SwitchPlaceWithEmpty(Block a, Block empty)
        {
            Node node_A = a.Parent_Node;
            Node node_B = empty.Parent_Node;

            a.Parent_Node = node_B;
            a.transform.SetParent(node_B.GetComponentInParent<Board>().transform);
            empty.Parent_Node = node_A;
            empty.transform.SetParent(node_A.GetComponentInParent<Board>().transform);

            node_A.current_Block = empty;
            node_B.current_Block = a;

            empty.transform.position = node_A.transform.position;
            a.MoveBlockToNode();
        }

        private void SwitchPlaces(Block a, Block b)
        {
            Node node_A = a.Parent_Node;
            Node node_B = b.Parent_Node;

            a.Parent_Node = node_B;
            a.transform.SetParent(node_B.GetComponentInParent<Board>().transform);
            b.Parent_Node = node_A;
            b.transform.SetParent(node_A.GetComponentInParent<Board>().transform);

            node_A.current_Block = b;
            node_B.current_Block = a;

            b.MoveBlockToNode();
            a.MoveBlockToNode();
        }

        // Tries Merging the Blocks and Spawns the resulting Block
        private void MergeBlocks(Block a, Block b, int result_Block_ID)
        {
            Node node_A = a.Parent_Node;
            Node node_B = b.Parent_Node;

            var empty = block_Manager.Create_Empty_Block();
            var new_Block = block_Manager.Create_Block(result_Block_ID);
            // Save the position from a before it gets Destroyed in Replace
            Vector2 l_Pos = a.transform.position;

            ReplaceBlock(node_A, empty);
            ReplaceBlock(node_B, new_Block);

            new_Block.transform.position = l_Pos;
            new_Block.MoveBlockToNode();
        }

        private void PushOtherBlockAway(Block a, Block b)
        {
            var free_nodes = nodes
                                .Where(n => n.current_Block.IsEmpty)
                                .OrderBy(b => Random.value)
                                .ToList();

            if (free_nodes.Count > 0)
            {
                Block old_empty_Block = free_nodes[0].current_Block;
                SwitchPlaceWithEmpty(b, old_empty_Block);
                SwitchPlaceWithEmpty(a, old_empty_Block);
            }
            else
            {
                SwitchPlaces(a, b);
            }
        }

        public int SellBlock(Block p_To_Sell_Block)
        {
            int value = p_To_Sell_Block.block_Data.value;

            RemoveBlock(p_To_Sell_Block);

            return value;
        }
    }

}