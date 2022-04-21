using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    protected Quaternion LookRotation;
    public Vector3 Shootposition;
    protected Vector3 DirectionTurret;
    protected float ShootCooldown;
    public bool _CanShoot;


    [SerializeField] public CircleCollider2D _DetectionRange;
    [SerializeField] public Transform _ShellStartPosition;
    [SerializeField] public Shell _Shell;
    [SerializeField] public float _FireRate;
    [SerializeField] public float _TurretRotationSpeed;



    // Start is called before the first frame update
    void Start()
    {
        ShootCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CanShoot();

    }
    public void SetShootPosition()
    {
        Shootposition = InputPlayer.Instance.m_posSourisWorld;

    }
    public void OrientationTurret()
    {
        Debug.Log("Shootposition1" + Shootposition);

        DirectionTurret = (Shootposition - _ShellStartPosition.position).normalized;
        Debug.Log("Shootposition2" + Shootposition);

        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        Debug.Log("Shootposition3" + Shootposition);

        LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        Debug.Log("Shootposition4" + Shootposition);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
    }
    public void CanShoot()
    {
        if (ShootCooldown > _FireRate)
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
        Shell ShellClone = Instantiate(_Shell, new Vector3(_ShellStartPosition.position.x, _ShellStartPosition.position.y, _ShellStartPosition.position.z), Quaternion.Euler(0, 0, -90) * transform.rotation);
        Vector3 aimedPoint = Shootposition;
        ShellClone.GetComponent<Shell>().LaunchProjectile(aimedPoint);
        ResetShootCooldown();
        _CanShoot = false;
    }
    public void ResetShootCooldown()
    {
        ShootCooldown = 0f;
    }
}
