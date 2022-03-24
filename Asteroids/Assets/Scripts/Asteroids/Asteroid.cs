using System;
using UnityEngine;
using UnityEngine.UI;


public class Asteroid : MonoBehaviour
{
    #region Nested types

    public enum AsteroidType
    {
        None    = 0,
        Small   = 1,
        Medium  = 2,
        Large   = 3,
        Huge    = 4
    }

    #endregion



    #region Fields
    
    private const float RotationSpeed = 0.7f;
    private const float MoveSpeed = 0.5f;

    [SerializeField] private AsteroidType asteroidType;
    [SerializeField] private Image noMineralsImage;
    [SerializeField] private Image mineralsImage;

    private int weaponLayerMask;
    
    #endregion



    #region Properties

    public AsteroidType Type => asteroidType;

    #endregion


    
    #region Unity lifecycle

    private void Awake()
    {
        weaponLayerMask = LayerMask.NameToLayer("Weapon");
        if (noMineralsImage && mineralsImage)
        {
            SelectMineralsTexture();
        }
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * MoveSpeed);
        
        noMineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
        mineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcessOnTriggerEnter(other);
    }

    #endregion



    #region Private methods

    private void SelectMineralsTexture()
    {
        System.Random random = new System.Random();
        int decision = random.Next(0, 2);
            
        noMineralsImage.gameObject.SetActive(decision == 0);
        mineralsImage.gameObject.SetActive(decision != 0);
    }


    private void ProcessOnTriggerEnter(Collider2D inputCollider)
    {
        if (inputCollider.gameObject.layer == weaponLayerMask)
        {
            gameObject.SetActive(false);
        }
    }
    
    #endregion
}
