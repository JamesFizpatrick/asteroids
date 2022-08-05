using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Game
{
    public class ShipVisualAppearanceController : MonoBehaviour
    {
        #region Fields

        private const float BlinkingFrequency = 0.25f;

        [SerializeField] private Image image;
      
        private Coroutine blinkingCoroutine;

        #endregion



        #region Public methods

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

        #endregion



        #region Private methods

        private IEnumerator BlinkCoroutine()
        {
            while (true)
            {
                image.gameObject.SetActive(false);
                yield return new WaitForSeconds(BlinkingFrequency);
                image.gameObject.SetActive(true);
                yield return new WaitForSeconds(BlinkingFrequency);
            }
        }

        #endregion
    }
}
