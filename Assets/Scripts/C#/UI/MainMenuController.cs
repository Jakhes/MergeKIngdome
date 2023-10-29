using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EvolvingCode.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private UIDocument doc;
        private Button start_Button;
        private Button surprise_Button;
        private Button quit_Button;

        private void Awake()
        {
            doc = GetComponent<UIDocument>();

            start_Button = doc.rootVisualElement.Q<Button>("StartButton");
            surprise_Button = doc.rootVisualElement.Q<Button>("SurpriseButton");
            quit_Button = doc.rootVisualElement.Q<Button>("QuitButton");

            start_Button.clicked += StartButtonOnClicked;
            surprise_Button.clicked += SaveLoadButtonOnClicked;
            quit_Button.clicked += QuitButtonOnClicked;
        }

        void OnDestroy()
        {
            start_Button.clicked -= StartButtonOnClicked;
            surprise_Button.clicked -= SaveLoadButtonOnClicked;
            quit_Button.clicked -= QuitButtonOnClicked;
        }

        private void StartButtonOnClicked()
        {
            SceneManager.LoadScene(1);
        }

        private void SaveLoadButtonOnClicked()
        {
            Debug.Log("Nothing happened what a surprise!");
        }

        private void QuitButtonOnClicked()
        {
            Application.Quit();
        }
    }
}
