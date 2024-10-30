using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using VoodooDoodoo.Gameplay;

namespace VoodooDoodoo.UI
{
    public sealed class WinPopup : BasePopup
    {
        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private List<Transform> stars;

        [Title("", "Stars delay")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float delayBeforeShowStars = 0.5f;
        [SerializeField, SuffixLabel("seconds", true)]
        private float delayBetweenStars = 0.2f;
        [Title("", "Stars thresholds")]
        [SerializeField, SuffixLabel("%", true)]
        private float oneStarThreshold = 0.3f;
        [SerializeField, SuffixLabel("%", true)]
        private float twoStarsThreshold = 0.5f;

        private WaitForSeconds waitDelayBetweenStars;
        private WaitForSeconds waitDelayBeforeShowStars;

        public override void Init ()
        {
            base.Init();

            waitDelayBetweenStars = new WaitForSeconds(delayBetweenStars);
            waitDelayBeforeShowStars = new WaitForSeconds(delayBeforeShowStars);
        }

        [Button]
        public void ShowWinPopup (float remainingTime, float maxTime)
        {
            SetTime(remainingTime);
            ShowStars(remainingTime, maxTime);
            string winText = GetWinText(remainingTime, maxTime);
            ShowPopup(winText);
        }

        private void SetTime (float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            timeText.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        private void ShowStars (float remainingTime, float maxTime)
        {
            DisableAllStars();

            float percentage = remainingTime / maxTime;
            int starsToShow = GetStarsToShow();

            StartCoroutine(ShowStarsDelayed());

            IEnumerator ShowStarsDelayed ()
            {
                yield return waitDelayBeforeShowStars;

                for (int i = 0; i < starsToShow; i++)
                {
                    Transform star = stars[i];
                    star.transform.localScale = Vector3.zero;
                    star.DOScale(1.0f, 0.65f).SetEase(Ease.OutBack);

                    yield return waitDelayBetweenStars;
                }
            }

            int GetStarsToShow ()
            {
                if (percentage < oneStarThreshold)
                    return 1;

                if (percentage < twoStarsThreshold)
                    return 2;
                return 3;
            }
        }

        private string GetWinText (float remainingTime, float maxTime)
        {
            float percentage = remainingTime / maxTime;

            if (percentage < oneStarThreshold)
                return "Good job!";

            if (percentage < twoStarsThreshold)
                return "Great job!";
            return "Amazing job!";
        }

        private void DisableAllStars ()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].localScale = Vector3.zero;
            }
        }

        public void NextLevel ()
        {
            HidePopup();
            GameManager.Instance.NextLevel();
        }
    }
}