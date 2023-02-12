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
    [SerializeField] private bool load_Save_Game = true;
    [SerializeField] private PlayerData player_Data;

    private void Start()
    {
        boardManager.InitBoards();
        LoadGame();




        boardManager.FocusBoard(0);
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
        FileHandler.SaveToJSON<PlayerData>(player_Data, "playerSave.json");
    }

    public void LoadGame()
    {
        isLoading = true;
        boardManager.LoadBoards(load_Save_Game);

        player_Data = FileHandler.ReadFromJSON<PlayerData>("playerSave.json");
        if (player_Data != default(PlayerData))
        {
            gold_Text.text = "Gold: " + player_Data.gold;
            _days_Text.text = "Day " + player_Data.days;
        }
        else
        {
            player_Data = new PlayerData(0, 0);
            gold_Text.text = "Gold: " + player_Data.gold;
            _days_Text.text = "Day " + player_Data.days;
        }
        isLoading = false;
    }

    public void SellBlock(Block to_Sell_Block)
    {
        player_Data.gold += main_Board.SellBlock(to_Sell_Block);
        gold_Text.text = "Gold: " + player_Data.gold;
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
        }
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
