using Sirenix.OdinInspector;
using UnityEngine;
using VoodooDoodoo.Puzzle;
using VoodooDoodoo.UI;
using VoodooDoodoo.Utils.Input;

namespace VoodooDoodoo.Gameplay
{
    public sealed class GameplayController : MonoBehaviour
    {
        [Title("General Settings")]
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private LayerMask puzzleElementLayer;

        [Title("Gameplay Elements")]
        [SerializeField]
        [InlineEditor]
        private PuzzleLevelData currentLevel;

        [SerializeField]
        private CollectedPuzzleElementsRow collectedPuzzleElementsRow;

        private readonly RaycastHit[] hits = new RaycastHit[4];

        private PuzzleElement currentSelectedElement;
        private PuzzleElement lastSelectedElement;

        private bool isActionPerformed;

        public void Init ()
        {
            collectedPuzzleElementsRow.Init();
        }
        
        private void OnEnable ()
        {
            InputController.OnPrimaryActionClickEvent += OnPrimaryActionClick;
        }

        private void OnDisable ()
        {
            InputController.OnPrimaryActionClickEvent -= OnPrimaryActionClick;
        }
        
        public void ResetLevel ()
        {
            collectedPuzzleElementsRow.ResetRow();
        }

        public void SetLevel (PuzzleLevelData levelData)
        {
            collectedPuzzleElementsRow.ResetRow();
            collectedPuzzleElementsRow.EnterAnimatePlatforms();
            currentLevel = levelData;
        }

        private void OnPrimaryActionClick (bool isActive)
        {
            isActionPerformed = isActive;

            if (isActive == false)
            {
                HandleReleaseAction();
            }
        }

        private void HandleReleaseAction ()
        {
            if (currentSelectedElement != null && collectedPuzzleElementsRow.IsAllPlatformOccupied() == false)
            {
                collectedPuzzleElementsRow.AddElement(currentSelectedElement);

                if (collectedPuzzleElementsRow.IsFullRow() == false)
                {
                    UIManager.Instance.requirementCardsContainer.UpdateRequirementCard(currentSelectedElement.elementData);

                    if (IsLevelCompleted())
                    {
                        CompleteLevel();
                    }
                }
                else
                {
                    FailLevel();
                }

                currentSelectedElement = null;
            }
        }

        private bool IsLevelCompleted ()
        {
            return currentLevel.IsLevelCompleted();
        }
        
        private void CompleteLevel ()
        {
            currentLevel.isCompleted = true;
            PopupManager.Instance.ShowWinPopup(currentLevel.remainingTime, currentLevel.timeLimit);
            UIManager.Instance.timeLimitController.StopTimer();
        }

        private void FailLevel ()
        {
            currentLevel.isFailed = true;
            PopupManager.Instance.ShowLosePopup("Out of space! :(");
            UIManager.Instance.timeLimitController.StopTimer();
        }

        private void Update ()
        {
            CheckPuzzleElements();
        }

        private void CheckPuzzleElements ()
        {
            if (currentLevel.IsFinished == true)
            {
                HandleLastElement(null);
                currentSelectedElement = null;
                return;
            }

            if (isActionPerformed == false)
            {
                HandleLastElement(null);
                return;
            }
          
            Ray ray = mainCamera.ScreenPointToRay(InputController.PrimaryActionPosition);
            int hitCount = Physics.RaycastNonAlloc(ray, hits, Mathf.Infinity, puzzleElementLayer);

            if (hitCount == 0)
            {
                HandleLastElement(null);
                currentSelectedElement = null;
                return;
            }

            PuzzleElement closestElement = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.TryGetComponent(out PuzzleElement puzzleElement))
                {
                    CheckClosestElement(ref closestElement, ref closestDistance, i, puzzleElement);
                }
            }

            if (closestElement != null)
            {
                HandleNewFocusedElement(closestElement);
            }
        }

        private void CheckClosestElement (ref PuzzleElement closestElement, ref float closestDistance, int i, PuzzleElement puzzleElement)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, hits[i].point);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestElement = puzzleElement;
            }
        }

        private void HandleNewFocusedElement (PuzzleElement puzzleElement)
        {
            if (puzzleElement == lastSelectedElement)
            {
                return;
            }

            currentSelectedElement = puzzleElement;
            puzzleElement.SetOutline(true);

            HandleLastElement(puzzleElement);
        }

        private void HandleLastElement (PuzzleElement puzzleElement)
        {
            if (lastSelectedElement != null)
            {
                lastSelectedElement.SetOutline(false);
            }

            lastSelectedElement = puzzleElement;
        }
    }
}