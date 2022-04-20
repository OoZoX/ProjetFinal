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
        ParticulesOnMouvement();
        if (InputPlayer.Instance != null)
        {
            InputPlayer.Instance.m_GetMousePositionWorld();
            Shootposition = InputPlayer.Instance.m_posSourisWorld;
            if (_CanShoot == true)
            {
                ShootClick = InputPlayer.Instance.m_clickMouseRight;
                if (ShootClick == true)
                {
                    ThrowProjectile();
                }
            }
        }
        if (m_cheminDep.Count > 0 && m_isMoving)
        {
            StartCoroutine(Deplacement());
        }
    }

    
    private IEnumerator Deplacement()
    {
        while (m_isMoving)
        {
            
            if((int)transform.position.x == m_cheminDep[1].x && (int)transform.position.y == m_cheminDep[1].y)
                m_cheminDep.RemoveAt(0);

            if (m_cheminDep.Count <= 1)
                m_isMoving = false;
            else
            {
                Rotation();

                Vector2 Force = new Vector2(_tankMoveSpeed, 0);
                transform.GetComponent<Rigidbody2D>().AddForce(Force);

            }

            yield return null;
        }
        
    }

    private void Rotation()
    {
        Vector3 Direction = (m_cheminDep[1] - m_cheminDep[0]).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * Direction;
        Quaternion Rotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, Time.deltaTime * _tankRotationSpeed);
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
