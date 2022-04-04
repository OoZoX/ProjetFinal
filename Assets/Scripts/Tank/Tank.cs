using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _tankSprite;
    [SerializeField] private GameObject _turret;
    [SerializeField] private bool _shoot;
    [SerializeField] private GameObject _projectile;
    [SerializeField] public float _speed, _areaRadius, _fireRate;
    [SerializeField] public float _counter;
    [SerializeField] public SpriteRenderer _shootPosition;
    private int _health;
    public float rotationSpeed;
    private Quaternion lookRotation;
    private Vector3 directionTurret;

    // Start is called before the first frame update
    void Start()
    {
        _health = 30;



    }

    // Update is called once per frame
    void Update()
    {
         if(_health == 0)
         {
            Destroy(gameObject);
         }
        OrientationTurret();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Shell"))
        {
            _health = _health - 1;
            Debug.Log("hit _health : " + _health);
        }

    }
    private void FixedUpdate()
    {
        if (_shoot)
        {
            ThrowProjectile();
        }


    }

    private void ThrowProjectile()
    {
        //Debug.Log("_counter " + _counter);
        //Debug.Log("_fireRate " + _fireRate);
        if (_counter > _fireRate)
        {
            Debug.Log("shoot");
            GameObject clone = Instantiate(_projectile, transform.position,Quaternion.Euler(0,0,- 90) * _turret.transform.rotation );
            Vector3 aimedPoint = _shootPosition.transform.position;
            Debug.Log(_turret.transform.rotation.z + " _turret.transform.rotation.z ");
            clone.GetComponent<Obus>().LaunchProjectile(aimedPoint);

            _counter = 0f;
        }
        else
        {
            _counter += Time.deltaTime;
        }
         
    }

    private void OrientationTurret()
    {
        
        directionTurret = (_shootPosition.transform.position - _turret.transform.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * directionTurret;
        lookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        Debug.Log("directionTurret " + directionTurret);
        Debug.Log("lookRotation " + lookRotation);




    }
}
