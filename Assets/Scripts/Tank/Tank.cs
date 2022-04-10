using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _tankSprite;
    [SerializeField] private GameObject _turret;
    [SerializeField] private bool _shoot;
    [SerializeField] private GameObject _projectile;
    [SerializeField] public float _speed, _areaRadius, _fireRate;
    [SerializeField] public float _health;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public Animator _TankAnimator;
    [SerializeField] public Slider _slider;
    [SerializeField] public Canvas _canvas;
    private Quaternion lookRotation;
    private Vector3 directionTurret;
    private float _counter;
    private bool _click = false;
    private Vector3 _Shootposition;
    private float MaxHealth = 5;
    private float testcalcul;
    // Start is called before the first frame update
    void Start()
    {
        _counter = 0;
        MaxHealth = 5;
        ActualizeHealthBar();

    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();

        if (_health == 0)
        {
            _TankAnimator.SetBool("Explosing", true);
            StartCoroutine(Explosion());
        }
         OrientationTurret();

        if(_shoot == true)
        {
           // InputPlayer.Instance.GetClickMouse();
            //_click = InputPlayer.Instance.m_clickMouseRight;
            //Debug.Log("_click " + _click);
            if (_click == true)
            {
                InputPlayer.Instance.m_GetMousePositionWorld();
                _Shootposition = InputPlayer.Instance.m_posSourisWorld;
                //Debug.Log("_Shootposition " + _Shootposition);
            }
        }
    }
    private IEnumerator Explosion()
    {
        _turret.SetActive(false);
        Destroy(gameObject, 0.9f);
        _canvas.enabled = false;
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

    private void FixedUpdate()
    {
        if (_shoot)
        {
            ThrowProjectile();

        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            CaptureZone.CaptureActuelle = CaptureZone.CaptureActuelle + 1f;
            Debug.Log("CaptureActuelle : " + CaptureZone.CaptureActuelle);
            Debug.Log("CaptureMax : " + CaptureZone.CaptureMax);
        }
    }
    private void ThrowProjectile()
    {
        if (_counter > _fireRate)
        {
            Debug.Log("shoot");
            GameObject clone = Instantiate(_projectile, new Vector3(_turret.transform.position.x , _turret.transform.position.y  , _turret.transform.position.z) ,Quaternion.Euler(0,0,- 90) * _turret.transform.rotation );
            Vector3 aimedPoint = _Shootposition;
            Debug.Log(_turret.transform.rotation.z + " _turret.transform.rotation.z ");
            clone.GetComponent<Obus>().LaunchProjectile(aimedPoint);
            _counter = 0f;
        }
        else
        {
            _counter += Time.deltaTime;
        }
         
    }

    private void OrientationTurret()
    {
        
        directionTurret = (_Shootposition - _turret.transform.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * directionTurret;
        lookRotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void ActualizeHealthBar()
    {
        _slider.value = _health / MaxHealth;

    }
}
