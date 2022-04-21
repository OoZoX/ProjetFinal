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
        ActualizeHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        //_turret.OrientationTurret();
        _turret.CanShoot();
        ParticulesOnMouvement();
        if (InputPlayer.Instance != null)
        {
            InputPlayer.Instance.m_GetMousePositionWorld();
            //_turret.SetShootPosition();
            if (_turret._CanShoot == true)
            {
                ShootClick = InputPlayer.Instance.m_clickMouseRight;
                if (ShootClick == true)
                {
                    _turret.ThrowProjectile();
                }
            }
        }
        if (m_startDep)
        {
            StopAllCoroutines();
            StartCoroutine(Deplacement());
            m_startDep = false;
        }
    }

    
    private IEnumerator Deplacement()
    {
        while (true)
        {
            Cell cellBelow = ManagerGraph.Instance.m_GetCellFromPosWorld(transform.position);
            Vector3 Direction = new Vector3(cellBelow.m_bestDirection.Vector.x, cellBelow.m_bestDirection.Vector.y, 0);
            Rotation(Direction);

            RaycastHit2D rayCast = Physics2D.Raycast(transform.position, new Vector2(0,0), 0.1f, m_layerMask);

            Vector2 ForceSpeed = new Vector2();
            if (rayCast.collider)
            {
                if (rayCast.collider.gameObject.layer == 6)
                {
                    ForceSpeed = Direction * _tankMoveSpeed * 0.75f;
                }
                else if (rayCast.collider.gameObject.layer == 7)
                {
                    ForceSpeed = Direction * _tankMoveSpeed * 0.5f;
                }
                else
                {
                    ForceSpeed = Direction * _tankMoveSpeed;
                }
            }
            else
            {
                ForceSpeed = Direction * _tankMoveSpeed;
            }



            Rigidbody2D tank = transform.GetComponent<Rigidbody2D>();

            tank.AddForce(ForceSpeed);

            yield return new WaitForFixedUpdate();
        }



        //while (m_isMoving)
        //{

        //    if((int)transform.position.x == m_cheminDep[1].x && (int)transform.position.y == m_cheminDep[1].y)
        //        m_cheminDep.RemoveAt(0);

        //    if (m_cheminDep.Count <= 1)
        //        m_isMoving = false;
        //    else
        //    {
        //        Rotation();

        //        Vector2 Force = new Vector2(_tankMoveSpeed, 0);
        //        transform.GetComponent<Rigidbody2D>().AddForce(Force);

        //    }

        //    yield return null;
        //}

    }

    private void Rotation(Vector3 Direction)
    {

        //Vector3 Direction = (CellDirection.m_bestDirection - transform.position).normalized;
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
