using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VoodooDoodoo.UI;

namespace VoodooDoodoo.MainMenu
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Transform titlePanel;
        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private Button startGameButton;

        [SerializeField]
        private TextMeshProUGUI goodLuckText;

        private void Start ()
        {
            ResetMainMenuObjects();

            ScreenFader.Instance.FadeIn(1.0f);
        }

        [Button(ButtonSizes.Gigantic)]
        private void ResetMainMenuObjects ()
        {
            startGameButton.transform.localScale = Vector3.zero;
            startGameButton.transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBack);
            
            goodLuckText.transform.localScale = Vector3.zero;

            titlePanel.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 3.3f);
            titlePanel.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, -3.3f), 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                titlePanel.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), 0.5f);
            });
        }

        public void StartGame ()
        {
            startGameButton.transform.DOScale(0.0f, 0.5f).SetEase(Ease.InBack);
            goodLuckText.transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBack);
            ScreenFader.Instance.FadeOut(1.0f, LoadGameplayScene);
        }

        private void LoadGameplayScene ()
        {
            SceneManager.LoadScene(1);
        }
    }
}