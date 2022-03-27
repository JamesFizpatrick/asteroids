using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Game
{
    public class ShipVisualAppearanceController : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Blink(Action onComplete)
        {
            StartCoroutine(BlinkCoroutine(onComplete));
        }
        
        private IEnumerator BlinkCoroutine(Action onComplete)
        {
            image.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            image.gameObject.SetActive(true);
            onComplete?.Invoke();
            yield return null;
        }
    }
}
