using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VoodooDoodoo.Puzzle;

namespace VoodooDoodoo.UI
{
    public sealed class TimeLimitController : MonoBehaviour
    {
       [SerializeField]
       private TextMeshProUGUI timeText;
       
       private PuzzleLevelData currentLevelData;
       
       private readonly StringBuilder timeStringBuilder = new();

       private readonly WaitForSeconds waitUpdateInterval = new(1.0f);

       public void StartTimer(PuzzleLevelData levelData)
       {
           currentLevelData = levelData;
           currentLevelData.remainingTime = currentLevelData.timeLimit;
           AnimateTimer();
           UpdateTimeText();
           StartCoroutine(UpdateTimer());
       }

       private void AnimateTimer ()
       {
           transform.localScale = Vector3.zero;
           transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBack);
       }
       
       public void StopTimer()
       {
           StopAllCoroutines();
       }

       private IEnumerator UpdateTimer()
       {
           while (currentLevelData.remainingTime > 0)
           {
               yield return waitUpdateInterval;
               currentLevelData.remainingTime -= 1;
               UpdateTimeText();
           }
           
           FailLevel();
       }
       
       private void FailLevel ()
       {
           currentLevelData.isFailed = true;
           PopupManager.Instance.ShowLosePopup("Times Up!");
           StopTimer();
       }

       private void UpdateTimeText()
       {
           TimeSpan timeSpan = TimeSpan.FromSeconds(currentLevelData.remainingTime);
           timeStringBuilder.Clear();
           timeStringBuilder.Append($"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}");
           timeText.text = timeStringBuilder.ToString();
       }
    }
}
