using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    
     [SerializeField] private float _projectileSpeed;
     [SerializeField] private Rigidbody2D _projectileRg2D;
     [SerializeField] private BoxCollider2D _ShellColliderRg2D;
     [SerializeField] private Animator _ShellAnimator;
     [SerializeField] private float _lifeTime;

     Quaternion toRotation;
     private bool IsDestroy;
     public void Start()
     {
        IsDestroy = false;
     }
    void Update() 
     {
        if (IsDestroy)
        {
            
            //StartCoroutine(Destroy());
        }
    }
    public void LaunchProjectile(Vector3 aimedPoint)
    {
        StartCoroutine(Fire(aimedPoint));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _ShellColliderRg2D.bounds.Intersects(collision.collider.bounds))
        {
            StartCoroutine(Destroy());
        }
        if (collision.gameObject.CompareTag("Tank") && _ShellColliderRg2D.bounds.Intersects(collision.collider.bounds))
        {
            StartCoroutine(Destroy());
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tank") && _ShellColliderRg2D.bounds.Intersects(collision.bounds))
        {
            StartCoroutine(Destroy());
        }
        if (collision.gameObject.CompareTag("Player") && _ShellColliderRg2D.bounds.Intersects(collision.bounds))
        {
            StartCoroutine(Destroy());
        }
        if (collision.gameObject.CompareTag("Shell") && _ShellColliderRg2D.bounds.Intersects(collision.bounds))
        {
            StartCoroutine(Destroy());
        }
    }
    private IEnumerator Destroy()
    {
        _ShellAnimator.SetBool("Explosing", true);
        _projectileRg2D.velocity = Vector2.zero;
        _projectileRg2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(this.gameObject, 0.5f);
        this.transform.localScale = new Vector2(0.3f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        this.transform.localScale = new Vector2(0.4f, 0.4f);
        yield return new WaitForSeconds(0.2f);
        this.transform.localScale = new Vector2(0.2f, 0.2f);
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
