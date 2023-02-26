using System.Collections;
using System.Collections.Generic;
using EvolvingCode.MergingBoard;
using UnityEngine;

namespace EvolvingCode
{
    public class DevCommands : MonoBehaviour
    {
        [SerializeField] private BoardManager boardManager;

        public void UseCommand(string p_Potential_Command)
        {
            switch (p_Potential_Command)
            {
                case "lock -1":
                    boardManager.LockBoard(-1);
                    break;
                case "lock 0":
                    boardManager.LockBoard(0);
                    break;
                case "lock 1":
                    boardManager.LockBoard(1);
                    break;
                case "unlock -1":
                    boardManager.UnlockBoard(-1);
                    break;
                case "unlock 0":
                    boardManager.UnlockBoard(0);
                    break;
                case "unlock 1":
                    boardManager.UnlockBoard(1);
                    break;
                default:
                    break;
            }
        }
    }
}
