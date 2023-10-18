using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolvingCode.MergingBoard;
using System;
using TMPro;
using System.IO;
using EvolvingCode.IngameMessages;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board main_Board;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TMP_Text gold_Text;
    [SerializeField] private TMP_Text _days_Text;
    [SerializeField] private SuccessMessageManager _SuccessMsgManager;
    [SerializeField] private WarningMessageManager _WarningMsgManager;
    [SerializeField] private InfoMessageManager _InfoMsgManager;

    private static bool isLoading = false;
    [SerializeField] private bool _Load_New_Game = false;
    [SerializeField] public PlayerData player_Data;

    private void Start()
    {
        boardManager.InitBoards();
        LoadGame();
    }

    void Update()
    {
        gold_Text.text = "Gold: " + player_Data.gold;
    }


    private void OnApplicationQuit()
    {
        if (!isLoading)
        {
            SaveGame();
        }
    }

    public void SaveGame()
    {
        boardManager.SaveBoards();
        FileHandler.SaveToJSON<PlayerData>(player_Data, "PlayerSave.json");
    }

    public void LoadGame()
    {
        isLoading = true;
        boardManager.LoadBoards(_Load_New_Game);


        if (_Load_New_Game)
        {
            player_Data = FileHandler.ReadFromJSON<PlayerData>("NewGamePlayerSave.json");
            gold_Text.text = "Gold: " + player_Data.gold;
            _days_Text.text = "Day " + player_Data.days;
        }
        else
        {
            player_Data = FileHandler.ReadFromJSON<PlayerData>("PlayerSave.json");
            if (player_Data == default(PlayerData))
                player_Data = FileHandler.ReadFromJSON<PlayerData>("NewGamePlayerSave.json");
            gold_Text.text = "Gold: " + player_Data.gold;
            _days_Text.text = "Day " + player_Data.days;
        }
        isLoading = false;
    }

    public void NewGame()
    {
        _Load_New_Game = true;
        LoadGame();
        _Load_New_Game = false;
    }

    public void SellBlock(Block to_Sell_Block)
    {
        player_Data.gold += main_Board.SellBlock(to_Sell_Block);
    }

    internal void UnlockBlock(Unlockable current_Selected_Block)
    {
        int l_Price_For_Unlocking = ((UnlockableData)(current_Selected_Block.block_Data)).price;

        if (l_Price_For_Unlocking <= player_Data.gold)
        {
            player_Data.gold -= l_Price_For_Unlocking;
            main_Board.UnlockBlock(current_Selected_Block);
        }
    }

    public void NextDay()
    {
        // Worker go to sleep
        if (boardManager.Next_Day_All_Boards())
        {
            // update Days counter
            player_Data.days += 1;
            _days_Text.text = "Day " + player_Data.days;
        }

        // Check if 30 Days have been reached
        if (player_Data.days > 30)
        {
            // GameOver
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    public void ProgressCastleState()
    {
        if (player_Data.castleProgress == CastleProgress.None)
        {
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(5, Vector2.one);
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(7, Vector2.one);
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(8, Vector2.one);
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(11, Vector2.one);
            boardManager.UnlockBoard(1);
            player_Data.castleProgress = CastleProgress.Tent;

            _SuccessMsgManager.CastleStageReached(Input.mousePosition, "Tent");
        }
        else if (player_Data.castleProgress == CastleProgress.Tent)
        {
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(14, Vector2.one);
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(48, Vector2.one);
            boardManager._Main_Board.board.Try_Spawning_Block_On_Board(12, Vector2.one);
            player_Data.castleProgress = CastleProgress.Hut;

            _SuccessMsgManager.CastleStageReached(Input.mousePosition, "Hut");
        }
    }
}

[Serializable]
public class PlayerData
{
    public int gold = 0;

    public int days = 1;

    public CastleProgress castleProgress = CastleProgress.None;

    public PlayerData(int pGold, int pDays, CastleProgress p_Castle_Progress)
    {
        this.gold = pGold;
        this.days = pDays;
        castleProgress = p_Castle_Progress;
    }
}

[Serializable]
public enum CastleProgress
{
    None,
    Tent,
    Hut,
    Hall,
    Castle
}
