using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //Gameobjects
    [SerializeField] public SpriteRenderer _tankSprite;
    [SerializeField] public Turret _turret;
    [SerializeField] public Slider _slider;
    [SerializeField] public Canvas _canvas;
    [SerializeField] public Animator _TankAnimator;
    [SerializeField] public BoxCollider2D _TankCollider;
    [SerializeField] public ParticleSystem _TankParticules;
    [SerializeField] public Rigidbody2D _TankBody;

    [SerializeField] private GameParameters _gameParameters;
    //stats
    [SerializeField] public float _Speed;
    [SerializeField] public float _Health;
    [SerializeField] public float _MaxHealth;
    [SerializeField] public float _tankMoveSpeed;
    [SerializeField] public float _CaptureSpeed;
    [SerializeField] public float _tankRotationSpeed;

    //variables

    protected GameObject EnnemyTank;

    protected bool DeathTrigger;


    public LayerMask m_layerMask;
    public bool m_startDep = false;
    public bool m_isMoving = false;

    public static Tank Instance;
    // Start is called before the first frame update
    void Start()
    {
        _tankRotationSpeed = _gameParameters.TankTurnSpeed;
        DeathTrigger = false;
    }


    protected IEnumerator DeathExplosion()
    {
        DeathTrigger = true;
        Debug.Log("Begin Animation Tank Death");
        //_turret.gameObject.SetActive(false);
        this._canvas.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 1f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this._tankSprite.transform.localScale = new Vector2(0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(3f);
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
            _Health = _Health -collision.GetComponent<Shell>()._ShellDammage;
        }
    }
    protected void GetHeal(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heal") && _TankCollider.bounds.Intersects(collision.bounds))
        {
            _Health = _Health + collision.transform.parent.GetComponent<HealingItem>()._HealingValue; ;
            Destroy(collision.gameObject);
            if (_Health > _MaxHealth)
            {
                _Health = _MaxHealth;
            }
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
