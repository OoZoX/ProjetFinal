using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTank : Tank
{
    protected bool ShootClick;
    //public Vector3 Shootposition;
    // Start is called before the first frame update
    void Start()
    {
        ShootClick = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        Shoot();
        RotateTurret();
        if (m_startDep)
        {
            StopAllCoroutines();
            StartCoroutine(Deplacement());
            m_startDep = false;
        }
    }

    private void RotateTurret()
    {
        InputPlayer.Instance.m_GetMousePositionWorld();
        Vector3 posMouse = InputPlayer.Instance.m_posSourisWorld;
        _turret.OrientationTurret(posMouse);
    }
    private void Shoot()
    {

        ShootClick = InputPlayer.Instance.m_clickMouseRight;
        if (ShootClick == true && InputPlayer.Instance.m_KeyboardN)
        {
            _turret.ThrowProjectile();
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetHeal(collision);
        CapturingZone(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CapturingZone(collision);
    }
}
