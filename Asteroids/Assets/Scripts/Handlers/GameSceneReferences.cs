using UnityEngine;


public class GameSceneReferences : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas mainCanvas;
    
    public static Camera MainCamera { get; private set; }
    public static Canvas MainCanvas { get; private set; }

    private void Awake()
    {
        MainCamera = mainCamera;
        MainCanvas = mainCanvas;
    }
}
