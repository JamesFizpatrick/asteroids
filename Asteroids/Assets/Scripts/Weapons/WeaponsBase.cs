using UnityEngine;


public class WeaponsBase : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float fireCooldown;


    public float FireCooldown => fireCooldown;
    
    
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed);
    }
}
