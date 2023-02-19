using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolvingCode.MergingBoard
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private List<Board_Infos> boards;
        public Board current_Board;

        public void FocusBoard(int id)
        {
            Board new_Board = boards.Find(n => n.board_Id == id).board;
            if (new_Board != null && current_Board != new_Board)
            {
                // Fade out old Board
                //if (current_Board != null)
                //{
                //    current_Board.FadeOutBoard();
                //}
                // Fade to new Board
                current_Board = new_Board;
                //new_Board.gameObject.SetActive(true);
                //new_Board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                new_Board.FocusOnBoard();
            }
            else
            {
                Debug.Log("Could not find Board with ID : " + id);
            }
        }

        internal void InitBoards()
        {
            foreach (var item in boards)
            {
                item.board.InitiateBoard();
            }
        }

        public void SaveBoards()
        {
            boards.ForEach(n => FileHandler.SaveToJSON<BoardData>(n.board.saveBoard(), n.board_Save_Name + ".json"));
        }

        public void LoadBoards(bool p_Load_New_Game)
        {
            if (p_Load_New_Game)
            {
                boards.ForEach(n => n.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>("NewGame" + n.board_Save_Name + ".json")));
            }
            else
            {
                boards.ForEach(n => n.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>(n.board_Save_Name + ".json")));
            }
        }

        public void Sleep_All_Boards()
        {
            boards.ForEach(n => n.board.Sleep());
        }
    }

    [Serializable]
    public struct Board_Infos
    {
        public int board_Id;
        public Board board;
        public string board_Save_Name;
        public string new_Game_Board;
    }
}
