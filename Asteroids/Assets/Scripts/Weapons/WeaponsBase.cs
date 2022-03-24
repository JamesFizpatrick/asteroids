using System;
using UnityEngine;


public class WeaponsBase : MonoBehaviour
{
    #region Fields

    [SerializeField] private float speed;
    [SerializeField] private float fireCooldown;

    private int asteroidLayer;
    
    #endregion


    
    #region Properties

    public float FireCooldown => fireCooldown;

    #endregion


    
    #region Unity lifecycle

    private void Awake() => asteroidLayer = LayerMask.NameToLayer("Asteroid");
    
    
    private void FixedUpdate() => transform.Translate(Vector3.up * speed);


    private void OnTriggerEnter2D(Collider2D col)
    {
        ProcessOnTriggerEnter(col);
    }
    
    #endregion


    
    #region Private metohds

    private void ProcessOnTriggerEnter(Collider2D col)
    {
        if (col.gameObject.layer == asteroidLayer)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion
}
