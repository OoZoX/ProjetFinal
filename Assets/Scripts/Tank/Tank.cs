using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _tankSprite;
    [SerializeField] private SpriteRenderer _turretSprite;
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
        _health = 3;
        rotationSpeed = 1f;


    }

    // Update is called once per frame
    void Update()
    {
         if(_health == 0)
         {
            Destroy(gameObject);
         }  

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
            OrientationTurret();

    }

    private void ThrowProjectile()
    {
        //Debug.Log("_counter " + _counter);
        //Debug.Log("_fireRate " + _fireRate);
        if (_counter > _fireRate)
        {
            Debug.Log("shoot");
            GameObject clone = Instantiate(_projectile, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Vector3 aimedPoint = _shootPosition.transform.position;
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
        directionTurret = (_shootPosition.transform.position - _turretSprite.transform.position).normalized;
        lookRotation = Quaternion.LookRotation(directionTurret);
        _turretSprite.transform.rotation = Quaternion.Slerp(_turretSprite.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        Debug.Log("directionTurret " + directionTurret);
        Debug.Log("lookRotation " + lookRotation);
        //_turretSprite.transform.Rotate()

    }
}
