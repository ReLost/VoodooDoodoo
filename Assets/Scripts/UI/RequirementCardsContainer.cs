using UnityEngine;
using VoodooDoodoo.Puzzle;

namespace VoodooDoodoo.UI
{
    public sealed class RequirementCardsContainer : MonoBehaviour
    {
        [SerializeField]
        private RequirementCard[] cards;

        public void SetAllRequirementCards (PuzzleLevelData levelData)
        {
            DisableAllCards();

            for (int i = 0; i < levelData.requirementToFinishLevel.Length; i++)
            {
                PuzzleLevelElementData puzzleLevelElementData = levelData.requirementToFinishLevel[i];
                puzzleLevelElementData.missingElementsCount = puzzleLevelElementData.count;
                SetCard(i, puzzleLevelElementData);
            }
        }

        private void DisableAllCards ()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].DisableCard();
            }
        }

        private void SetCard (int index, PuzzleLevelElementData elementData)
        {
            cards[index].SetCard(elementData);
        }
        
        public void UpdateRequirementCard (PuzzleElementData elementData)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].IsCardForElement(elementData))
                {
                    cards[i].UpdateCardCounter();
                    break;
                }
            }
        }
    }
}