using Sirenix.OdinInspector;
using UnityEngine;
using VoodooDoodoo.Puzzle;
using VoodooDoodoo.UI;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.Gameplay
{
    public sealed class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [SerializeField]
        private PuzzleSpawner puzzleSpawner;
        [SerializeField]
        private GameplayController gameplayController;

        [SerializeField]
        private PuzzleLevelData[] levels;

        private PuzzleLevelData currentLevel;
        [SerializeField]
        private int currentLevelIndex;

        [Title("DEBUG")]
        [SerializeField, InlineEditor]
        private PuzzleLevelData debugLevel;
        
        private void Start ()
        {
            currentLevelIndex = 0;
            StartGame(levels[currentLevelIndex]);
            ScreenFader.Instance.FadeIn(3.0f);

            gameplayController.Init();
        }

        public void StartGame (PuzzleLevelData levelData)
        {
            currentLevel = levelData;

            currentLevel.isCompleted = false;
            currentLevel.isFailed = false;

            puzzleSpawner.SetLevel(currentLevel);
            puzzleSpawner.StartCurrentLevel();

            gameplayController.SetLevel(currentLevel);

            UIManager.Instance.timeLimitController.StartTimer(currentLevel);
            UIManager.Instance.requirementCardsContainer.SetAllRequirementCards(currentLevel);
        }

        [Button(ButtonSizes.Medium)]
        private void DEBUG_StartDebugLevel ()
        {
            StartGame(debugLevel);
        }

        public void RestartLevel ()
        {
            gameplayController.ResetLevel();
            StartGame(currentLevel);
        }

        public void NextLevel ()
        {
            currentLevelIndex++;
            currentLevelIndex %= levels.Length;
            PuzzleLevelData nextLevel = currentLevelIndex < levels.Length ? levels[currentLevelIndex] : levels[0];
            puzzleSpawner.ClearLevel();
            StartGame(nextLevel);
        }
    }
}