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
        if (collision.gameObject.CompareTag("Shell") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            _Health = _Health - _ShellDammage;
            Debug.Log("hit _health : " + _Health);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            CaptureZone.Instance.CaptureActuelle = CaptureZone.Instance.CaptureActuelle + _CaptureSpeed;
        }
    }
}
