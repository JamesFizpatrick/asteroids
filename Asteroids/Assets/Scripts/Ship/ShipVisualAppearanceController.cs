using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Game
{
    public class ShipVisualAppearanceController : MonoBehaviour
    {
        [SerializeField] private Image image;

        private const float blinkingFrequency = 0.25f;
        
        private Coroutine blinkingCoroutine;
        
        
        public void StartToBlink()
        {
            StopToBlink();
            blinkingCoroutine = StartCoroutine(BlinkCoroutine());
        }


        public void StopToBlink()
        {
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
            }
        }
        
        
        private IEnumerator BlinkCoroutine()
        {
            while (true)
            {
                image.gameObject.SetActive(false);
                yield return new WaitForSeconds(blinkingFrequency);
                image.gameObject.SetActive(true);
                yield return new WaitForSeconds(blinkingFrequency);
            }
        }
    }
}
