using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolvingCode.MergingBoard;
using System;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board main_Board;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TMP_Text gold_Text;
    [SerializeField] private TMP_Text _days_Text;

    private static bool isLoading = false;
    [SerializeField] private bool _Load_New_Game = false;
    [SerializeField] public PlayerData player_Data;

    private void Start()
    {
        boardManager.InitBoards();
        LoadGame();




        boardManager.FocusBoard(0);
    }

    void Update()
    {
        gold_Text.text = "Gold: " + player_Data.gold;
    }

    public void Focus(int id)
    {
        boardManager.FocusBoard(id);
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

    public void NextDay()
    {
        // Worker go to sleep
        boardManager.Sleep_All_Boards();
        // update Days counter
        player_Data.days += 1;
        _days_Text.text = "Day " + player_Data.days;
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
}

[Serializable]
public class PlayerData
{
    public int gold = 0;

    public int days = 1;

    public PlayerData(int pGold, int pDays)
    {
        this.gold = pGold;
        this.days = pDays;
    }
}
