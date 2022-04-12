using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IATank : Tank
{
    protected bool EnnemyInRange;
    // Start is called before the first frame update
    void Start()
    {
        EnnemyInRange = false;
        ShootCooldown = 0;
        _CanShoot = false;
        ActualizeHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        CanShoot();
        if (EnnemyInRange == false)
        {
            IddleTank();
        }
        else
        {
             OrientationTurret();
            if (_CanShoot == true)
            {
                ThrowProjectile();
            }
        }     
    }
    private void IddleTank()
    {
        //DirectionTurret = (Shootposition - _ShellStartPosition.position).normalized;
        //Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        //LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        //_turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            Shootposition = collision.transform.position;
            ShootCooldown = 0f;
            _CanShoot = false;
            EnnemyInRange = true;
        }
        if (collision.gameObject.CompareTag("Shell") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            _Health = _Health - _ShellDammage;
            Debug.Log("hit _health : " + _Health);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            Shootposition = collision.transform.position;
            EnnemyInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnnemyInRange = false;
    }
   
}
