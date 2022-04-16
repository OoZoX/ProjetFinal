using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTank : Tank
{
    protected bool ShootClick;

    // Start is called before the first frame update
    void Start()
    {
        ShootCooldown = 0;
        ShootClick = false;
        _CanShoot = false;
        ActualizeHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        OrientationTurret();
        CanShoot();
        if (InputPlayer.Instance != null)
        {
            InputPlayer.Instance.m_GetMousePositionWorld();
            Shootposition = InputPlayer.Instance.m_posSourisWorld;
            if (_CanShoot == true)
            {
                InputPlayer.Instance.GetClickMouse();
                ShootClick = InputPlayer.Instance.m_clickMouseRight;
                if (ShootClick == true)
                {
                    ThrowProjectile();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetHitShell(collision);
        GetHeal(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CapturingZone(collision);
    }
}
