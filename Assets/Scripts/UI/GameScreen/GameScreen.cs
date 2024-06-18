using System;
using Services.DI.Context;
using Services.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.GameScreen
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button gameFieldButton;
        [SerializeField] private Button backButton;

        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private Image levelImage;
        private GameData _gameData;

        private void Start()
        {
            _gameData = ProjectContext.GetInstance<GameData>();
            counterText.text = _gameData.CurrentLevel.Progress.Counter.ToString();
            gameFieldButton.onClick.AddListener(DecreaseCounter);
            backButton.onClick.AddListener(ExitToMenu);
            levelImage.sprite = _gameData.CurrentLevel.Resources.Data.Background;
        }

        private void DecreaseCounter()
        {
            int decreasedValue = Convert.ToInt32(counterText.text) - 1;
            counterText.text = decreasedValue.ToString();
            _gameData.CurrentLevel.Progress.Counter = decreasedValue;
            _gameData.CurrentLevel.Progress.Save();
            if (decreasedValue <= 0)
            {
                _gameData.CurrentLevel.Progress.IsComplete = true;
                ExitToMenu();
            }
            
        }

        private void ExitToMenu()
        {
            SaveData();
            SceneManager.LoadScene("MainMenu");
        }

        private void SaveData()
        {
            _gameData.CurrentLevel.Progress.Save();
        }
    }
}
