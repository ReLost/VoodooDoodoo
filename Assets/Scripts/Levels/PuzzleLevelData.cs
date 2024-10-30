using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VoodooDoodoo.Puzzle
{
    [CreateAssetMenu(fileName = "PuzzleLevel00", menuName = "VoodooDoodoo/PuzzleLevelData")]
    public sealed class PuzzleLevelData : ScriptableObject
    {
        [Title("Requirements")]
        public PuzzleLevelElementData[] requirementToFinishLevel;

        [SuffixLabel("seconds", true), PropertySpace(10)]
        public int timeLimit = 300;

        [Title("Level Content")]

        [InfoBox("Only multiples of 3 are allowed to each element count.")]
        public PuzzleLevelElementData[] puzzleLevelElements;

        [HideInInspector]
        public float remainingTime;

        [HideInInspector]
        public bool isCompleted;
        [HideInInspector]
        public bool isFailed;
        public bool IsFinished => isCompleted || isFailed;

        public bool IsLevelCompleted ()
        {
            for (int i = 0; i < requirementToFinishLevel.Length; i++)
            {
                if (requirementToFinishLevel[i].missingElementsCount > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

    [Serializable]
    public class PuzzleLevelElementData
    {
        public PuzzleElementData elementData;

        [Tooltip("Only multiples of 3 are allowed.")]
        [InlineButton(nameof(IncrementCount), "+")]
        [InlineButton(nameof(DecrementCount), "-")]
        [OnValueChanged(nameof(OnCountChanged))]
        public int count;

        [HideInInspector]
        public int missingElementsCount;

        private void IncrementCount ()
        {
            count += 3;
        }

        private void DecrementCount ()
        {
            count -= 3;
        }

        private void OnCountChanged ()
        {
            count = (int)Math.Round(count / 3.0) * 3;
        }
    }
}