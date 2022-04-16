using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //Gameobjects
    [SerializeField] private SpriteRenderer _tankSprite;
    [SerializeField] private GameObject _turret;
    [SerializeField] private Slider _slider;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Animator _TankAnimator;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _ShellStartPosition;

    //stats
    [SerializeField] private bool _CanShoot;
    [SerializeField] private float _speed, _areaRadius, _fireRate;
    [SerializeField] private float _health;
    [SerializeField] private float _MaxHealth;
    [SerializeField] private float _TurretRotationSpeed;
    [SerializeField] private float _CaptureSpeed;
    [SerializeField] private float _ShellDammage;

    //variables
    private Quaternion LookRotation;
    private Vector3 Shootposition;
    private Vector3 DirectionTurret;
    private float ShootCooldown;
    private bool ShootClick;
    public bool IsBot; 

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
        if (IsBot == false)
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
        else
        {

        }
    }
    private void CanShoot()
    {
        if (ShootCooldown > _fireRate)
        {
            _CanShoot = true;
        }
        else
        {
            ShootCooldown += Time.deltaTime;
        }
    }
    private IEnumerator DeathExplosion()
    {
        _turret.SetActive(false);
        Destroy(gameObject, 0.9f);
        _canvas.gameObject.SetActive(false);
        _tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.6f);
        _tankSprite.transform.localScale = new Vector2(0.9f, 0.9f);
        yield return new WaitForSeconds(0.2f);
        _tankSprite.transform.localScale = new Vector2(0.7f, 0.7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shell"))
        {
            _health = _health - 1;
            Debug.Log("hit _health : " + _health);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            CaptureZone.Instance.CaptureActuelle = CaptureZone.Instance.CaptureActuelle + _CaptureSpeed;
        }
    }
    private void ThrowProjectile()
    {
        GameObject clone = Instantiate(_projectile, new Vector3(_ShellStartPosition.position.x, _ShellStartPosition.position.y, _ShellStartPosition.position.z), Quaternion.Euler(0, 0, -90) * _turret.transform.rotation);
        Vector3 aimedPoint = Shootposition;
        clone.GetComponent<Obus>().LaunchProjectile(aimedPoint);
        ShootCooldown = 0f;
        _CanShoot = false;
    }

    private void OrientationTurret()
    {
        DirectionTurret = (Shootposition - _ShellStartPosition.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
        Debug.Log("_turret.transform.rotation " + _turret.transform.rotation);
    }

    public void ActualizeHealthBar()
    {
        _slider.value = _health / _MaxHealth;
        if (_health == 0)
        {
            _TankAnimator.SetBool("Explosing", true);
            StartCoroutine(DeathExplosion());
        }
    }
}
