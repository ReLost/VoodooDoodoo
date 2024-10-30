using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoodooDoodoo.Puzzle;

namespace VoodooDoodoo.UI
{
    public sealed class RequirementCard : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TextMeshProUGUI countText;
        [SerializeField]
        private Image completedImage;

        private PuzzleLevelElementData currentLevelElementData;
        
        [Title("Animation Settings", "Appearing Card Animation")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float appearingCardDuration = 1.5f;
        [SerializeField]
        private Ease appearingCardEase = Ease.OutBack;
        
        [Title("", "Counter Scale Animation")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float counterScaleDuration = 0.5f;
        [SerializeField]
        private Ease counterScaleEase = Ease.InExpo;

        [Title("", "Completed Image Animation")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float completedImageScaleDuration = 0.5f;
        [SerializeField]
        private Ease completedImageScaleEase = Ease.OutBack;
        [SerializeField, SuffixLabel("seconds", true)]
        private float completedImageScaleDelay = 0.4f;

        [Title("", "Card Animation")]
        [SerializeField]
        private float cardScaleIncreaseSize = 1.1f;
        [SerializeField, SuffixLabel("seconds", true)]
        private float cardScaleDuration = 0.6f;
        [SerializeField]
        private Ease cardScaleEase = Ease.OutBack;
        [SerializeField, PropertySpace(10)]
        private float cardRotateAmount = 2.0f;
        [SerializeField, SuffixLabel("seconds", true)]
        private float cardRotateDuration = 0.5f;
        [SerializeField, SuffixLabel("seconds", true), PropertySpace(10)]
        private float cardScaleHorizontalDelay = 0.4f;
        [SerializeField, SuffixLabel("seconds", true)]
        private float cardScaleHorizontalDuration = 0.5f;
        [SerializeField]
        private Ease cardScaleHorizontalEase = Ease.InExpo;

        public void SetCard (PuzzleLevelElementData levelElementData)
        {
            currentLevelElementData = levelElementData;
            ResetCard();
            gameObject.SetActive(true);
            image.sprite = levelElementData.elementData.sprite;
            countText.text = levelElementData.missingElementsCount.ToString();
        }

        private void ResetCard ()
        {
            transform.DOKill();
            transform.localScale = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            completedImage.transform.DOKill();
            completedImage.transform.localScale = Vector3.zero;
            
            countText.transform.DOKill();
            countText.transform.localScale = Vector3.one;
            
            transform.DOScale(1.0f, appearingCardDuration).SetEase(appearingCardEase);
        }

        public void UpdateCardCounter ()
        {
            currentLevelElementData.missingElementsCount--;
            currentLevelElementData.missingElementsCount = Mathf.Clamp(currentLevelElementData.missingElementsCount, 0, currentLevelElementData.missingElementsCount);
            countText.text = currentLevelElementData.missingElementsCount.ToString();

            if (currentLevelElementData.missingElementsCount == 0)
            {
                AnimateCompletedCard();
            }
        }

        private void AnimateCompletedCard ()
        {
            countText.transform.DOScale(0f, counterScaleDuration).SetEase(counterScaleEase);

            completedImage.transform.DOScale(1.0f, completedImageScaleDuration).SetEase(completedImageScaleEase).SetDelay(completedImageScaleDelay).OnComplete(() =>
            {
                transform.DOScale(cardScaleIncreaseSize, cardScaleDuration).SetEase(cardScaleEase);
                transform.DOLocalRotate(new Vector3(0.0f, cardRotateAmount * 360.0f, 0.0f), cardRotateDuration, RotateMode.FastBeyond360);
                transform.DOScaleX(0.0f, cardScaleHorizontalDuration).SetEase(cardScaleHorizontalEase).SetDelay(cardScaleHorizontalDelay).OnComplete(DisableCard);

                //TODO: Some VFX?
            });
        }

        public bool IsCardForElement (PuzzleElementData elementData)
        {
            if (currentLevelElementData == null)
            {
                return false;
            }

            return currentLevelElementData.elementData == elementData;
        }

        public void DisableCard ()
        {
            gameObject.SetActive(false);
        }
    }
}