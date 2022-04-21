using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //Gameobjects
    [SerializeField] public SpriteRenderer _tankSprite;
    [SerializeField] public GameObject _turret;
    [SerializeField] public Slider _slider;
    [SerializeField] public Canvas _canvas;
    [SerializeField] public Animator _TankAnimator;
    [SerializeField] public GameObject _Shell;
    [SerializeField] public Transform _ShellStartPosition;
    [SerializeField] public CircleCollider2D _DetectionRange;
    [SerializeField] public BoxCollider2D _TankCollider;
    [SerializeField] public ParticleSystem _TankParticules;
    [SerializeField] public Rigidbody2D _TankBody;

    [SerializeField] private GameParameters _gameParameters;
    //stats
    [SerializeField] public bool _CanShoot;
    [SerializeField] public float _Speed;
    [SerializeField] public float _FireRate;
    [SerializeField] public float _Health;
    [SerializeField] public float _MaxHealth;
    [SerializeField] public float _tankRotationSpeed;
    [SerializeField] public float _TurretRotationSpeed;
    [SerializeField] public float _tankMoveSpeed;
    [SerializeField] public float _CaptureSpeed;
    [SerializeField] public float _ShellDammage;

    //variables
    protected Quaternion LookRotation;
    protected Vector3 Shootposition;
    protected Vector3 DirectionTurret;
    protected GameObject EnnemyTank;
    protected float ShootCooldown;
    protected bool DeathTrigger;



    public bool m_startDep = false;
    public bool m_isMoving = false;

    public static Tank Instance;
    // Start is called before the first frame update
    void Start()
    {
        _tankRotationSpeed = _gameParameters.TankTurnSpeed;
        DeathTrigger = false;
    }

    protected void CanShoot()
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
    protected IEnumerator DeathExplosion()
    {
        DeathTrigger = true;
        Debug.Log("Begin Animation Tank Death");
        this._turret.SetActive(false);
        this._canvas.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 1f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this._tankSprite.transform.localScale = new Vector2(0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.2f);
    }

    protected void ThrowProjectile()
    {
        GameObject ShellClone = Instantiate(_Shell, new Vector3(_ShellStartPosition.position.x, _ShellStartPosition.position.y, _ShellStartPosition.position.z), Quaternion.Euler(0, 0, -90) * _turret.transform.rotation);
        Vector3 aimedPoint = Shootposition;
        ShellClone.GetComponent<Shell>().LaunchProjectile(aimedPoint);
        ShootCooldown = 0f;
        _CanShoot = false;
    }

    protected void OrientationTurret()
    {
        DirectionTurret = (Shootposition - _ShellStartPosition.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
    }

    protected void ActualizeHealthBar()
    {
        _slider.value = _Health / _MaxHealth;
        if (_Health == 0 && DeathTrigger == false)
        {
            _TankAnimator.SetBool("Explosing", true);
            StartCoroutine(DeathExplosion());
        }
    }
    protected void CapturingZone(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            var zone = collision.transform.parent.GetComponent<CaptureZone>();
            zone.CaptureActuelle = zone.CaptureActuelle + _CaptureSpeed;
        }
    }
    protected void GetHitShell(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shell") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            _Health = _Health - _ShellDammage;
        }
    }
    protected void GetHeal(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heal") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            _Health = _Health + 1;
            Destroy(collision.gameObject);
            if (_Health > _MaxHealth)
            {
                _Health = _MaxHealth;
            }
            Debug.Log("hit _health : " + _Health);
        }
    }

    protected void ParticulesOnMouvement()
    {
        if (_TankBody.velocity.y ==  0 && _TankBody.velocity.x == 0)
        {
            _TankParticules.enableEmission = false;
        }
        else
        {
            _TankParticules.enableEmission = true;
        }

    }
}
