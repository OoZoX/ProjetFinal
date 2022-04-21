using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //Gameobjects
    [SerializeField] public GameObject _tankSprite;
    [SerializeField] public Turret _turret;
    [SerializeField] public UITank _UITank;

    [SerializeField] public Animator _TankAnimator;
    [SerializeField] public BoxCollider2D _TankCollider;
    [SerializeField] public ParticleSystem _TankParticules;
    [SerializeField] public Rigidbody2D _TankBody;

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
        DeathTrigger = false;
    }


    public IEnumerator DeathExplosion()
    {
        DeathTrigger = true;
        Debug.Log("Begin Animation Tank Death");
        //_turret.gameObject.SetActive(false);
        this._UITank.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 1f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this._tankSprite.transform.localScale = new Vector2(0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(3f);
    }

    public void Domage(int domage)
    {
        _Health -= domage;
        
    }

    public void Rotation(Vector3 Direction)
    {

        //Vector3 Direction = (CellDirection.m_bestDirection - transform.position).normalized;
        Vector3 upwardsdirection = Quaternion.Euler(0, 0, 90) * Direction;
        Quaternion Rotation = Quaternion.LookRotation(Vector3.forward, upwardsdirection);
        _tankSprite.gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, Time.deltaTime * _tankRotationSpeed);
    }
    public IEnumerator Deplacement()
    {
        while (true)
        {
            Cell cellBelow = ManagerGraph.Instance.m_GetCellFromPosWorld(transform.position);
            Vector3 Direction = new Vector3(cellBelow.m_bestDirection.Vector.x, cellBelow.m_bestDirection.Vector.y, 0);
            Rotation(Direction);
            _UITank.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 0.8f, transform.position.z);
            RaycastHit2D rayCast = Physics2D.Raycast(transform.position, new Vector2(0, 0), 0.1f, m_layerMask);

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


}
