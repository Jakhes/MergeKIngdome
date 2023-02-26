using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using TMPro;

namespace EvolvingCode.MergingBoard
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] public Board_Infos _Main_Board;
        [SerializeField] private List<Board_Infos> _Side_Boards;
        [SerializeField] private Button _Go_Up_Button;
        [SerializeField] private Button _Go_Down_Button;
        [SerializeField] private Vector3 _Up_Target_Pos;
        [SerializeField] private Vector3 _Center_Target_Pos;
        [SerializeField] private Vector3 _Down_Target_Pos;
        [SerializeField] private float _Board_Fade_Move_Speed_Time;
        [SerializeField] private float _Board_Fade_Shrink_Speed_Time;
        [SerializeField] private TMP_Text _Board_Name_Text;



        public int current_Side_Board;
        private Side_Board_State _current_Side_Board_State;

        void Update()
        {
            if (current_Side_Board <= 0)
            {
                _Go_Up_Button.interactable = false;
            }
            else
            {
                _Go_Up_Button.interactable = true;
            }
            if (current_Side_Board >= _Side_Boards.Count - 1)
            {
                _Go_Down_Button.interactable = false;
            }
            else
            {
                _Go_Down_Button.interactable = true;
            }
            _Board_Name_Text.text = _Side_Boards[current_Side_Board].board.name;
        }

        internal void InitBoards()
        {
            _Go_Up_Button.interactable = false;
            current_Side_Board = 0;
            _Main_Board.board.InitiateBoard();
            foreach (var item in _Side_Boards)
            {
                item.board.InitiateBoard();
                item.board.transform.parent.transform.position = _Up_Target_Pos;
                item.board.transform.parent.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            }
            _Main_Board.board.FocusOnBoard();
            FadeInBoard(false, _Side_Boards[1].board.transform.parent, _Side_Boards[0].board.transform.parent);

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

        public bool Next_Day_All_Boards()
        {
            if (_current_Side_Board_State == Side_Board_State.Normal_State)
            {
                _Main_Board.board.NextDay();
                _Side_Boards.ForEach(n => n.board.NextDay());
                return true;
            }
            return false;
        }

        public void SideBoardGoUp()
        {
            int l_New_Side_Board_ID = current_Side_Board - 1;
            FadeInBoard(false, _Side_Boards[current_Side_Board].board.transform.parent, _Side_Boards[l_New_Side_Board_ID].board.transform.parent);
            current_Side_Board -= 1;
        }

        public void SideBoardGoDown()
        {
            int l_New_Side_Board_ID = current_Side_Board + 1;
            FadeInBoard(true, _Side_Boards[current_Side_Board].board.transform.parent, _Side_Boards[l_New_Side_Board_ID].board.transform.parent);
            current_Side_Board += 1;
        }

        async void FadeInBoard(bool p_Fade_Up, Transform p_Fade_Out, Transform p_Fade_In)
        {
            _current_Side_Board_State = Side_Board_State.Moving_State;

            if (p_Fade_Up)
            {
                p_Fade_Out.transform.DOScale(new Vector3(0.01f, 0.01f, _Board_Fade_Shrink_Speed_Time), 1);
                p_Fade_Out.transform.DOMoveY(_Up_Target_Pos.y, _Board_Fade_Move_Speed_Time);
                p_Fade_In.transform.position = _Down_Target_Pos;
                p_Fade_In.transform.DOScale(new Vector3(1f, 1f, 1), _Board_Fade_Shrink_Speed_Time);
                await p_Fade_In.transform.DOMoveY(_Center_Target_Pos.y, _Board_Fade_Move_Speed_Time).AsyncWaitForCompletion();
                _current_Side_Board_State = Side_Board_State.Normal_State;
            }
            else
            {
                p_Fade_Out.transform.DOScale(new Vector3(0.01f, 0.01f, _Board_Fade_Shrink_Speed_Time), 1);
                p_Fade_Out.transform.DOMoveY(_Down_Target_Pos.y, _Board_Fade_Move_Speed_Time);
                p_Fade_In.transform.position = _Up_Target_Pos;
                p_Fade_In.transform.DOScale(new Vector3(1f, 1f, 1), _Board_Fade_Shrink_Speed_Time);
                await p_Fade_In.transform.DOMoveY(_Center_Target_Pos.y, _Board_Fade_Move_Speed_Time).AsyncWaitForCompletion();
                _current_Side_Board_State = Side_Board_State.Normal_State;
            }
        }

        public void UnlockBoard(int p_Board_ID)
        {
            if (p_Board_ID == -1)
            {
                _Main_Board.board.UnlockBoard();
            }
            else if (p_Board_ID >= 0 && p_Board_ID < _Side_Boards.Count)
            {
                _Side_Boards[p_Board_ID].board.UnlockBoard();
            }
        }
        public void LockBoard(int p_Board_ID)
        {
            if (p_Board_ID == -1)
            {
                _Main_Board.board.LockBoard();
            }
            else if (p_Board_ID >= 0 && p_Board_ID < _Side_Boards.Count)
            {
                _Side_Boards[p_Board_ID].board.LockBoard();
            }
        }
    }

    [Serializable]
    public struct Board_Infos
    {
        public int board_Id;
        public Board board;
        public string board_Save_Name;
    }

    public enum Side_Board_State
    {
        Normal_State,
        Moving_State
    }
}
