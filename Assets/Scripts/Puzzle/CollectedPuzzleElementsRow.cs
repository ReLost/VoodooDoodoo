using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.Puzzle
{
    public sealed class CollectedPuzzleElementsRow : MonoBehaviour
    {
        [SerializeField]
        private List<CollectedPuzzleElementPlatform> platforms;

        [Title("Animation Settings")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float delayBetweenPlatformsAppearing = 0.1f;

        [SerializeField, SuffixLabel("seconds", true)]
        private float timeElementToPlatform = 0.5f;
        [SerializeField, SuffixLabel("seconds", true)]
        private float timeToElementShiftOnPlatform = 0.25f;

        private WaitForSeconds delayBetweenPlatformsAppearingWait;
        private WaitForSeconds timeToCheckThreeInRowWait;
        private WaitForSeconds timeToRemoveThreeInRowAfterAnimationWait;

        private List<CollectedPuzzleElementPlatform> shuffledPlatforms = new();

        private bool isDeletingThreeInRow;

        public void Init ()
        {
            delayBetweenPlatformsAppearingWait = new WaitForSeconds(delayBetweenPlatformsAppearing);
            timeToCheckThreeInRowWait = new WaitForSeconds(timeElementToPlatform);
            timeToRemoveThreeInRowAfterAnimationWait = new WaitForSeconds(0.4f);
        }

        public void EnterAnimatePlatforms ()
        {
            StartCoroutine(AnimateProcess());

            IEnumerator AnimateProcess ()
            {
                shuffledPlatforms = platforms.Shuffle();
                PrepareAllPlatforms();

                foreach (CollectedPuzzleElementPlatform platform in shuffledPlatforms)
                {
                    platform.EnterAnimate();

                    yield return delayBetweenPlatformsAppearingWait;
                }
            }

            void PrepareAllPlatforms ()
            {
                foreach (CollectedPuzzleElementPlatform platform in shuffledPlatforms)
                {
                    platform.PrepareForEnterAnimate();
                }
            }
        }

        public void AddElement (PuzzleElement element)
        {
            StartCoroutine(AddingElementProcess());

            IEnumerator AddingElementProcess ()
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (IsTheSameElementAlreadyExist(element, i))
                    {
                        int lastIndex = i;

                        FindProperPlatformForElement(i, ref lastIndex, element);

                        ShiftElements(lastIndex);

                        if (lastIndex + 1 < platforms.Count)
                        {
                            platforms[lastIndex + 1].SetElement(element, timeElementToPlatform, 2, true);

                            yield return timeToCheckThreeInRowWait;

                            CheckThreeInRow();
                            yield break;
                        }
                    }
                }

                for (int i = 0; i < platforms.Count; i++)
                {
                    CollectedPuzzleElementPlatform platform = platforms[i];

                    if (platform.isOccupied == false)
                    {
                        platform.SetElement(element, timeElementToPlatform, 2, true);

                        yield break;
                    }
                }
            }
        }
        
        private bool IsTheSameElementAlreadyExist (PuzzleElement element, int index)
        {
           return platforms[index].isOccupied && platforms[index].collectedElement.elementData == element.elementData;
        }

        private void FindProperPlatformForElement (int index, ref int properPlatformIndex, PuzzleElement element)
        {
            for (int j = index + 1; j < platforms.Count; j++)
            {
                if (platforms[j].isOccupied && platforms[j].collectedElement.elementData == element.elementData)
                {
                    properPlatformIndex = j;
                }
                else
                {
                    break;
                }
            }
        }

        private void ShiftElements (int index)
        {
            for (int k = platforms.Count - 1; k > index + 1; k--)
            {
                if (platforms[k - 1].isOccupied)
                {
                    platforms[k].SetElement(platforms[k - 1].collectedElement, timeToElementShiftOnPlatform, 1);
                }
                else
                {
                    platforms[k].ClearPlatform();
                }
            }
        }
        
        public bool IsAllPlatformOccupied ()
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].isOccupied == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFullRow ()
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].isOccupied == false)
                {
                    return false;
                }
            }

            for (int i = 0; i < platforms.Count - 2; i++)
            {
                if (AreThreeConsecutiveElementsIsOccupied(i) && AreThreeConsecutiveElementsSame(i))
                {
                    return false;
                }
            }

            return true;
        }

        public void ResetRow ()
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].RemoveElement();
                platforms[i].ResetVisualsToDefault();
            }
        }

        private void CheckThreeInRow ()
        {
            if (isDeletingThreeInRow == false)
            {
                StartCoroutine(RemovingThreeInRowProcess());
            }

            IEnumerator RemovingThreeInRowProcess ()
            {
                for (int i = 0; i < platforms.Count - 2; i++)
                {
                    if (AreThreeConsecutiveElementsIsOccupied(i) && AreThreeConsecutiveElementsSame(i))
                    {
                        isDeletingThreeInRow = true;

                        yield return new WaitUntil(() => IsAnyThreeConsecutivePlatformsWaitingForElement(i));

                        MoveEdgeElementToCenterOfThree(i, platforms[i + 1].collectedElement.transform.position);
                        MoveEdgeElementToCenterOfThree(i + 2, platforms[i + 1].collectedElement.transform.position);

                        yield return timeToRemoveThreeInRowAfterAnimationWait;

                        RemoveThreeElements(i);
                        ShiftElementsToFirstFreePlaces(i);

                        isDeletingThreeInRow = false;

                        CheckThreeInRow();
                        yield break;
                    }
                }
            }

            bool IsAnyThreeConsecutivePlatformsWaitingForElement (int index)
            {
                return platforms[index].isWaitingForElement == false &&
                       platforms[index + 1].isWaitingForElement == false &&
                       platforms[index + 2].isWaitingForElement == false;
            }

            void MoveEdgeElementToCenterOfThree (int index, Vector3 centerPosition)
            {
                platforms[index].collectedElement.transform.DOLocalMoveZ(-4.0f, 0.15f)
                    .OnComplete(() => { platforms[index].collectedElement.transform.DOMove(centerPosition, 0.25f); });
            }

            void RemoveThreeElements (int index)
            {
                platforms[index].RemoveElement();
                platforms[index + 1].RemoveElement();
                platforms[index + 2].RemoveElement();
            }

            void ShiftElementsToFirstFreePlaces (int index)
            {
                for (int j = index + 3; j < platforms.Count; j++)
                {
                    if (platforms[j].collectedElement != null)
                    {
                        platforms[j - 3].SetElement(platforms[j].collectedElement, timeToElementShiftOnPlatform, 1);
                        platforms[j].ClearPlatform();
                    }
                }
            }
        }

        private bool AreThreeConsecutiveElementsIsOccupied (int index)
        {
            return platforms[index].isOccupied && platforms[index + 1].isOccupied && platforms[index + 2].isOccupied;
        }

        private bool AreThreeConsecutiveElementsSame (int index)
        {
            return platforms[index].collectedElement.elementData == platforms[index + 1].collectedElement.elementData &&
                   platforms[index].collectedElement.elementData == platforms[index + 2].collectedElement.elementData;
        }
    }
}