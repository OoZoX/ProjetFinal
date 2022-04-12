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
    [SerializeField] private GameObject _Shell;
    [SerializeField] private Transform _ShellStartPosition;
    [SerializeField] private CircleCollider2D _DetectionRange;
    [SerializeField] private BoxCollider2D _TankCollider;

    //stats
    [SerializeField] private bool _CanShoot;
    [SerializeField] private float _Speed;
    [SerializeField] private float _FireRate;
    [SerializeField] private float _Health;
    [SerializeField] private float _MaxHealth;
    [SerializeField] private float _TurretRotationSpeed;
    [SerializeField] private float _CaptureSpeed;
    [SerializeField] private float _UnCaptureSpeed;
    [SerializeField] private float _ShellDammage;

    //variables
    private Quaternion LookRotation;
    private Vector3 Shootposition;
    private Vector3 DirectionTurret;
    private GameObject EnnemyTank;
    private float ShootCooldown;
    private bool ShootClick;
    private bool EnnemyInRange;
    public bool IsBot; 


    // Start is called before the first frame update
    void Start()
    {
        EnnemyInRange = false;
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
        if (IsBot == false && InputPlayer.Instance != null)
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
        else 
        {
            _CaptureSpeed = _UnCaptureSpeed;
            if (_CanShoot == true && EnnemyInRange == true)
            {
                ThrowProjectile();
            }
        }
    }
    private void CanShoot()
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tank") && IsBot == true)
        {
            Shootposition = collision.transform.position;
            ShootCooldown = 0f;
            _CanShoot = false;
            EnnemyInRange = true;
        }
        if (collision.gameObject.CompareTag("Shell") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            //_TankAnimator.SetBool("Explosing", true);
            //collision.gameObject.SetActive(false);
            collision.enabled = false;
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
        if (collision.gameObject.CompareTag("Tank") && IsBot == true)
        {
            Shootposition = collision.transform.position;
            EnnemyInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnnemyInRange = false;
    }
    private void ThrowProjectile()
    {
        GameObject ShellClone = Instantiate(_Shell, new Vector3(_ShellStartPosition.position.x, _ShellStartPosition.position.y, _ShellStartPosition.position.z), Quaternion.Euler(0, 0, -90) * _turret.transform.rotation);
        Vector3 aimedPoint = Shootposition;
        ShellClone.GetComponent<Shell>().LaunchProjectile(aimedPoint);
        ShootCooldown = 0f;
        _CanShoot = false;
    }

    private void OrientationTurret()
    {
        DirectionTurret = (Shootposition - _ShellStartPosition.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * DirectionTurret;
        LookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, LookRotation, Time.deltaTime * _TurretRotationSpeed);
    }

    public void ActualizeHealthBar()
    {
        _slider.value = _Health / _MaxHealth;
        if (_Health == 0)
        {
            _TankAnimator.SetBool("Explosing", true);
            StartCoroutine(DeathExplosion());
        }
    }
    private void ShellExplosion(Collider2D Shell)
    {

    }
}
