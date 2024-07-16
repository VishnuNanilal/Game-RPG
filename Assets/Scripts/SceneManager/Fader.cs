using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately()
        {
            canvasGroup.alpha = 1;
        }
        public IEnumerator FadeOut(float time)
        {
            float N = time / Time.deltaTime;
            while(canvasGroup.alpha<1)
            {
                canvasGroup.alpha += (1 / N);
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            float N = time / Time.deltaTime;
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= (1 / N);
                yield return null;
            }
        }
    }
}

