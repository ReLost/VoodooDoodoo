using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VoodooDoodoo.UI
{
    public class BasePopup : MonoBehaviour
    {
        [Title("References")]
        [SerializeField]
        protected DOTweenAnimation showAnimation;
        
        [SerializeField]
        protected TextMeshProUGUI titleText;
        
        [SerializeField]
        private Button actionButton;
        [SerializeField]
        private Button quitButton;
        
        [Title("Settings", "Buttons delay")]
        [SerializeField, SuffixLabel("seconds", true)]
        private float delayBeforeShowButtons = 1.5f;
        
        public virtual void Init ()
        {
            
        }
        
        [Button]
        public virtual void ShowPopup (string title)
        {
            ShowButtons();
            
            titleText.text = title;
            gameObject.SetActive(true);
            showAnimation.DORestart();
        }
        
        public void ReturnToMenu ()
        {
            HidePopup();
            ScreenFader.Instance.FadeOut(1.0f, LoadMainMenuScene);
        }
        
        private void LoadMainMenuScene ()
        {
            SceneManager.LoadScene(0);
        }
        
        protected virtual void ShowButtons ()
        {
            actionButton.transform.localScale = Vector3.zero;
            actionButton.transform.DOScale(1.0f, 0.65f).SetEase(Ease.OutBack).SetDelay(delayBeforeShowButtons);
            
            quitButton.transform.localScale = Vector3.zero;
            quitButton.transform.DOScale(1.0f, 0.65f).SetEase(Ease.OutBack).SetDelay(delayBeforeShowButtons + 0.25f);
        }
        
        protected virtual void HidePopup ()
        {
            showAnimation.DOPlayBackwards();
        }
    }
}
