using System.Collections.Generic;
using Services.DI.Context;
using Services.GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainScreen
{
    public class MainScreen : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private GameObject categoryTogglePrefab;

        [Header("Containers")] [SerializeField]
        private RectTransform levelsContainer;

        [SerializeField] private RectTransform categoriesContainer;
        private GameData _gameData;

        private List<LevelButton> _buttons = new List<LevelButton>();

        private void Start()
        {
            _gameData = ProjectContext.GetInstance<GameData>();
            if (_gameData.Config == null)
            {
                Debug.Log("Config empty");
                return;
            }

            foreach (var level in _gameData.Config.Levels)
            {
                var levelButton = Instantiate(levelButtonPrefab, levelsContainer);
                levelButton.SetData(new Level(level));
                levelButton.Click += clickedLevel =>
                {
                    _gameData.CurrentLevel = clickedLevel;
                    SceneManager.LoadScene("GameScene");
                };
                _buttons.Add(levelButton);
            }
        }
    }
}