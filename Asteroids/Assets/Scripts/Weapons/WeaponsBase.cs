using UnityEngine;


public class WeaponsBase : MonoBehaviour
{
    #region Fields

    [SerializeField] private float speed;
    [SerializeField] private float fireCooldown;

    #endregion


    
    #region Properties

    public float FireCooldown => fireCooldown;

    #endregion


    
    #region Unity lifecycle

    private void FixedUpdate() => transform.Translate(Vector3.up * speed);

    #endregion
}
