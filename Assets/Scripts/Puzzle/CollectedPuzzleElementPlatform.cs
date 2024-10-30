using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.Puzzle
{
    public sealed class CollectedPuzzleElementPlatform : MonoBehaviour
    {
        [Title("References")]
        [SerializeField]
        private Transform elementPosition;
        [SerializeField]
        private Renderer platformRenderer;

        [Title("Data")]
        [SerializeField]
        private Color defaultPlatformColor = new(1f, 0.94f, 0.57f);
        [SerializeField]
        public PuzzleElement collectedElement;
        [SerializeField]
        public bool isOccupied;
        [SerializeField]
        public bool isWaitingForElement;

        [Title("Animation Settings")]
        [SerializeField]
        private Vector3 targetPlatformScale = new(0.55f, 0.1f, 0.55f);
        [SerializeField, SuffixLabel("seconds", true)]
        private float scaleDuration = 1.0f;
        [SerializeField]
        private Color highlightedPlatformColor = new(1f, 0.5f, 0f);

        public void PrepareForEnterAnimate ()
        {
            transform.localScale = Vector3.zero;
        }

        public void EnterAnimate ()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(targetPlatformScale, scaleDuration).SetEase(Ease.OutBack).SetDelay(0.5f);
        }

        public void SetElement (PuzzleElement element, float animationDuration, int animationHeight, bool withAnimation = false)
        {
            collectedElement = element;
            CollectElementAnimation(element, animationDuration, animationHeight, withAnimation);
            isOccupied = true;
        }

        public void ClearPlatform ()
        {
            if (isOccupied)
            {
                collectedElement = null;
                isOccupied = false;
                ResetVisualsToDefault();
            }
        }

        public void RemoveElement ()
        {
            if (isOccupied)
            {
                collectedElement.gameObject.SetActive(false);
                collectedElement.ResetElement();
                collectedElement = null;
                isOccupied = false;
                ResetVisualsToDefault();
            }
        }

        private void CollectElementAnimation (PuzzleElement puzzleElement, float animationDuration, int height, bool withAnimation = false)
        {
            StartCoroutine(CollectElementAnimationProcess(puzzleElement, elementPosition, animationDuration, height, withAnimation));
        }

        private IEnumerator CollectElementAnimationProcess (PuzzleElement puzzleElement, Transform targetTransform, float animationDuration, int height, bool withAnimation = false)
        {
            float timer = 0.0f;
            puzzleElement.SetAsCollected(true);
            isWaitingForElement = true;
            Transform targetPuzzleElement = puzzleElement.transform;
            Vector3 startPosition = puzzleElement.transform.position;
            Quaternion startRotation = puzzleElement.transform.rotation;
            Vector3 startScale = puzzleElement.transform.localScale;
            Vector3 targetPosition = targetTransform.position;
            Quaternion targetRotation = targetTransform.localRotation;
            Vector3 targetScale = Vector3.one * 0.3f;

            while (timer < animationDuration)
            {
                float progress = timer / animationDuration;
                Vector3 position = VoodooDoodooExtensions.Parabola(startPosition, targetPosition, height, progress);
                Quaternion rotation = Quaternion.Slerp(startRotation, targetRotation, progress);
                Vector3 scale = Vector3.Lerp(startScale, targetScale, progress);
                targetPuzzleElement.position = position;
                targetPuzzleElement.rotation = rotation;
                targetPuzzleElement.localScale = scale;
                yield return null;

                timer += Time.deltaTime;
            }

            targetPuzzleElement.SetPositionAndRotation(targetPosition, targetRotation);
            targetPuzzleElement.localScale = targetScale;
            isWaitingForElement = false;

            if (withAnimation)
            {
                AnimatePlatform();
            }
        }

        private void AnimatePlatform ()
        {
            if (isOccupied)
            {
                transform.DOLocalMoveZ(-0.05f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                transform.DOScale(new Vector3(0.5f, 0.1f, 0.5f), 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                platformRenderer.material.DOColor(highlightedPlatformColor, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
        }

        public void ResetVisualsToDefault ()
        {
            transform.DOKill();
            transform.DOLocalMoveZ(0.0f, 0.1f);
            transform.DOScale(targetPlatformScale, 0.1f);

            platformRenderer.material.DOKill();
            platformRenderer.material.DOColor(defaultPlatformColor, 0.1f);
        }
    }
}