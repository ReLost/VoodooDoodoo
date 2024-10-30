using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.UI
{
    public sealed class ScreenFader : SingletonMonoBehaviour<ScreenFader>
    {
        [SerializeField]
        private Image faderImage;

        private Coroutine fadeCoroutine;

        protected override void Initialize ()
        {
            base.Initialize();

            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// From opaque to transparent
        /// </summary>
        [Button]
        public void FadeIn (float duration, Action callback = null)
        {
            TryStopFade();
            callback += DisableFader;
            fadeCoroutine = StartCoroutine(FadeProcess(0.0f, duration, callback));
        }

        /// <summary>
        /// From transparent to opaque
        /// </summary>
        [Button]
        public void FadeOut (float duration, Action callback = null)
        {
            TryStopFade();
            faderImage.gameObject.SetActive(true);
            fadeCoroutine = StartCoroutine(FadeProcess(1.0f, duration, callback));
        }

        private IEnumerator FadeProcess (float alpha, float duration, Action callback = null)
        {
            Color currentColor = faderImage.color;

            Color visibleColor = faderImage.color;
            visibleColor.a = alpha;
            
            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                faderImage.color = Color.Lerp(currentColor, visibleColor, counter / duration);
                yield return null;
            }
            
            callback?.Invoke();
        }

        private void TryStopFade ()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
        }

        private void DisableFader ()
        {
            faderImage.gameObject.SetActive(false);
        }
    }
}