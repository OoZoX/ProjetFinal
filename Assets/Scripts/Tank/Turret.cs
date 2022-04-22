using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    protected Quaternion LookRotation;
    public Vector3 Shootposition;
    protected Vector3 DirectionTurret;
    protected float ShootCooldown = 0;
    public bool _CanShoot;


    
    [SerializeField] public Transform _ShellStartPosition;
    [SerializeField] public Shell _Shell;
    [SerializeField] public float _FireRate;
    [SerializeField] public float _TurretRotationSpeed;



    // Update is called once per frame
    void Update()
    {
        CanShoot();

    }

    public void OrientationTurret(Vector3 shootPosition)
    {
        DirectionTurret = (shootPosition - transform.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
    }

    public void CanShoot()
    {
        if (ShootCooldown > _FireRate && SelectionTank.Instance.m_collider2DsTank.Contains(transform.parent.GetComponent<Collider2D>()))
        {
            _CanShoot = true;
        }
        else
        {
            ShootCooldown += Time.deltaTime;
        }
    }

    public void ThrowProjectile()
    {
        if (_CanShoot)
        {
            Shell ShellClone = Instantiate(_Shell, new Vector3(_ShellStartPosition.position.x, _ShellStartPosition.position.y, _ShellStartPosition.position.z), Quaternion.Euler(0, 0, -90) * transform.rotation);
            Vector3 aimedPoint = Shootposition;
            ShellClone.GetComponent<Shell>().LaunchProjectile(aimedPoint);
            ResetShootCooldown();
            _CanShoot = false;
        }
    }
    public void ResetShootCooldown()
    {
        ShootCooldown = 0f;
    }


}
