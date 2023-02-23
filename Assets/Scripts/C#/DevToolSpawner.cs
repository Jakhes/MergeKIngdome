using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EvolvingCode.MergingBoard;

namespace EvolvingCode
{
    public class DevToolSpawner : MonoBehaviour
    {
        public TMP_InputField inputField;
        public Board board;

        public void SpawnWithID()
        {
            string inputText = inputField.text;
            if (inputText.Length > 0) board.Try_Spawning_Block_On_Board(int.Parse(inputText), new Vector2());
        }
    }
}
