using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obus : MonoBehaviour
{
    
     [SerializeField] private float _projectileSpeed = 1f;
     [SerializeField] private Rigidbody2D _projectileRg2D;
     [SerializeField] private float _lifeTime = 3f;
     Quaternion toRotation;
     public void Start()
     {

     }
     void Update()
     {

     }
    public void LaunchProjectile(Vector3 aimedPoint)
    {
        StartCoroutine(Fire(aimedPoint));
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            Debug.Log("obus hit tank " );
            Destroy(gameObject);
        }     
    }
    
    private IEnumerator Fire(Vector3 aimedPoint)
      {
        Vector2 dir = transform.up;
        _projectileRg2D.AddForce(dir * _projectileSpeed, ForceMode2D.Impulse);

        StartCoroutine(DestroyProjectile());
        yield return null;
      }

      private IEnumerator DestroyProjectile()
      {
          yield return new WaitForSeconds(_lifeTime);
          Destroy(gameObject);
          yield return null;
      }
      
}
