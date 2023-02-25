using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EvolvingCode.MergingBoard
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private Board_Infos _Main_Board;
        [SerializeField] private List<Board_Infos> _Side_Boards;
        [SerializeField] private Button _Go_Up_Button;
        [SerializeField] private Button _Go_Down_Button;
        public int current_Side_Board;

        // public void FocusBoard(int id)
        // {
        //     Board new_Board = boards.Find(n => n.board_Id == id).board;
        //     if (new_Board != null && current_Board != new_Board)
        //     {
        //         // Fade out old Board
        //         //if (current_Board != null)
        //         //{
        //         //    current_Board.FadeOutBoard();
        //         //}
        //         // Fade to new Board
        //         current_Board = new_Board;
        //         //new_Board.gameObject.SetActive(true);
        //         //new_Board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
        //         new_Board.FocusOnBoard();
        //     }
        //     else
        //     {
        //         Debug.Log("Could not find Board with ID : " + id);
        //     }
        // }

        internal void InitBoards()
        {
            _Go_Up_Button.interactable = false;
            current_Side_Board = 0;
            _Main_Board.board.InitiateBoard();
            foreach (var item in _Side_Boards)
            {
                item.board.InitiateBoard();
                item.board.FadeOutBoard();
            }
            _Main_Board.board.FocusOnBoard();

            _Side_Boards[current_Side_Board].board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
        }

        public void SaveBoards()
        {
            FileHandler.SaveToJSON<BoardData>(_Main_Board.board.saveBoard(), _Main_Board.board_Save_Name + ".json");

            _Side_Boards.ForEach(n => FileHandler.SaveToJSON<BoardData>(n.board.saveBoard(), n.board_Save_Name + ".json"));
        }

        public void LoadBoards(bool p_Load_New_Game)
        {
            if (p_Load_New_Game)
            {
                _Main_Board.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>("NewGame" + _Main_Board.board_Save_Name + ".json"));

                _Side_Boards.ForEach(n => n.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>("NewGame" + n.board_Save_Name + ".json")));
            }
            else
            {
                _Main_Board.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>(_Main_Board.board_Save_Name + ".json"));

                _Side_Boards.ForEach(n => n.board.loadBoard(
                    FileHandler.ReadFromJSON<BoardData>(n.board_Save_Name + ".json")));
            }
        }

        public void Next_Day_All_Boards()
        {
            _Main_Board.board.NextDay();
            _Side_Boards.ForEach(n => n.board.NextDay());
        }

        public void SideBoardGoUp()
        {
            int l_New_Side_Board_ID = current_Side_Board - 1;
            if (l_New_Side_Board_ID < 0)
            {
                _Go_Up_Button.interactable = false;
            }
            else if (l_New_Side_Board_ID == 0)
            {
                _Go_Up_Button.interactable = false;
                _Go_Down_Button.interactable = true;
                _Side_Boards[current_Side_Board].board.FadeOutBoard();
                _Side_Boards[l_New_Side_Board_ID].board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                current_Side_Board -= 1;
            }
            else
            {
                _Go_Down_Button.interactable = true;
                _Side_Boards[current_Side_Board].board.FadeOutBoard();
                _Side_Boards[l_New_Side_Board_ID].board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                current_Side_Board -= 1;
            }
        }

        public void SideBoardGoDown()
        {
            int l_New_Side_Board_ID = current_Side_Board + 1;

            if (l_New_Side_Board_ID > _Side_Boards.Count)
            {
                _Go_Down_Button.interactable = false;
            }
            else if (l_New_Side_Board_ID == _Side_Boards.Count - 1)
            {
                _Go_Down_Button.interactable = false;
                _Go_Up_Button.interactable = true;
                _Side_Boards[current_Side_Board].board.FadeOutBoard();
                _Side_Boards[l_New_Side_Board_ID].board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                current_Side_Board += 1;
            }
            else
            {
                _Go_Up_Button.interactable = true;
                _Side_Boards[current_Side_Board].board.FadeOutBoard();
                _Side_Boards[l_New_Side_Board_ID].board.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                current_Side_Board += 1;
            }
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
