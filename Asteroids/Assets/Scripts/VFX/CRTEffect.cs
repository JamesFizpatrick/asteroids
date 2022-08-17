using UnityEngine;


namespace Asteroids.VFX
{
    public class CRTEffect : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Shader shader;

        [SerializeField] private float bend;
        [SerializeField] private float scanlineSize1;
        [SerializeField] private float scanlineSpeed1;
        [SerializeField] private float scanlineSize2;
        [SerializeField] private float scanlineSpeed2;
        [SerializeField] private float scanlineAmount;
        [SerializeField] private float vignetteSize;
        [SerializeField] private float vignetteSmoothness;
        [SerializeField] private float vignetteEdgeRound;
        [SerializeField] private float noiseSize;
        [SerializeField] private float noiseAmount;

        [SerializeField] private Vector2 redOffset;
        [SerializeField] private Vector2 blueOffset;
        [SerializeField] private Vector2 greenOffset;

        private Material material;

        #endregion



        #region Unity lifecycle

        private void Awake() => material = new Material(shader);


        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetFloat("Bend", bend);
            material.SetFloat("ScanlineSize1", scanlineSize1);
            material.SetFloat("ScanlineSpeed1", scanlineSpeed1);
            material.SetFloat("ScanlineSize2", scanlineSize2);
            material.SetFloat("ScanlineSpeed2", scanlineSpeed2);
            material.SetFloat("ScanlineAmount", scanlineAmount);
            material.SetFloat("VignetteSize", vignetteSize);
            material.SetFloat("VignetteSmoothness", vignetteSmoothness);
            material.SetFloat("VignetteEdgeRound", vignetteEdgeRound);
            material.SetFloat("NoiseSize", noiseSize);
            material.SetFloat("NoiseAmount", noiseAmount);

            material.SetVector("RedOffset", redOffset);
            material.SetVector("GreenOffset", blueOffset);
            material.SetVector("BlueOffset", greenOffset);

            Graphics.Blit(source, destination, material);
        }

        #endregion
    }
}