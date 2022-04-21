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
        ActualizeHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        ParticulesOnMouvement();
        if (EnnemyInRange == false)
        {
           
        }
        else
        {
            _turret.OrientationTurret();
            if (_turret._CanShoot == true)
            {
                _turret.ThrowProjectile();
            }
        }     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _turret.Shootposition = collision.transform.position;
            _turret.ResetShootCooldown();
            _turret._CanShoot = false;
            EnnemyInRange = true;
        }
        GetHitShell(collision);
        GetHeal(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _turret.Shootposition = collision.transform.position;
            EnnemyInRange = true;
        }
        CapturingZone(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnnemyInRange = false;
    }

}
