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
    [SerializeField] public GameObject _FullPrefab;

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

    private Cell[,] _Cell;
    private Cell _destionation;
    public LayerMask m_layerMask;

    public bool m_startDep = false;
    public bool m_isMoving = false;

    public static Tank Instance;
    // Start is called before the first frame update
    void Start()
    {

    }


    public IEnumerator DeathExplosion()
    {
        Debug.Log("Begin Animation Tank Death");
        this._TankAnimator.SetBool("Explosing", true);
        this._turret.gameObject.SetActive(false);
        this._UITank.gameObject.SetActive(false);
        Destroy(this.gameObject, 1.1f);
        Destroy(this._FullPrefab.gameObject, 1.1f);
        this._tankSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this._tankSprite.transform.localScale = new Vector2(0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this._tankSprite.transform.localScale = new Vector2(0.3f, 0.3f);   
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
        bool Moving = true;
        _Cell = ManagerGraph.Instance.m_tabCellMap;
        _destionation = ManagerGraph.Instance.m_cibleCell;
        while (Moving)
        {
            Cell cellBelow = ManagerGraph.Instance.m_GetCellFromPosWorld(transform.position);
            cellBelow = _Cell[cellBelow.m_gridIndex.x, cellBelow.m_gridIndex.y];
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
            Moving = ConditionStopMove();
        }

    }

    private bool ConditionStopMove()
    {
        if ((int)transform.position.x == )
        {

        }
        
        return true
    }

    protected void CapturingZone(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            var zone = collision.transform.parent.GetComponent<CaptureZone>();
            zone.CaptureActuelle = zone.CaptureActuelle + _CaptureSpeed;
        }
    }
    protected void GetHeal(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heal"))
        {
            _Health = _Health + collision.transform.GetComponent<HealingItem>()._HealingValue; 
            Destroy(collision.gameObject);
            if (_Health > _MaxHealth)
            {
                _Health = _MaxHealth;
            }
        }
    }


}
