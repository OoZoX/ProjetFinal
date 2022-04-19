using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTank : MonoBehaviour
{
    //[SerializeField]
    //private GameValue _gameValue;
    [SerializeField]
    LayerMask layerMask;

    
    private Vector3 _posMouseScreen;
    private Vector3 _posSquare;

    [SerializeField] Material _materialOutline;
    [SerializeField] Material _materialNormal;


    public List<Collider2D> m_collider2DsTank = new List<Collider2D>();

    private List<Collider2D> _collider2DsTankCopy = new List<Collider2D>();
    private List<Collider2D> _collider2DsTankDelete = new List<Collider2D>();

    

    public bool m_isClickLeft = false;
    public bool _firstClick = false;

    public static SelectionTank Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        Vector3 loacalVectorScale = new Vector3(0, 0, 0);
        transform.localScale = loacalVectorScale;

    }

    private void Update()
    {
        GetInput();
        CheckClickStart();
        UpdateSquare();
        CheckClickStop();
    }

    private void GetInput()
    {
        m_isClickLeft = InputPlayer.Instance.m_clickMouseLeft;
    }

    private void CheckClickStart()
    {
        if (m_isClickLeft && !_firstClick)
        {
            //Debug.Log($"<color=green> Start Detection Zone </color>");
            _firstClick = true;
         
            InputPlayer.Instance.m_GetMousePositionWorld();
            _posSquare = new Vector3(InputPlayer.Instance.m_posSourisWorld.x, InputPlayer.Instance.m_posSourisWorld.y, -1);
            transform.position = _posSquare;

            transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

            
        }
    }

    private void UpdateSquare()
    {
        if(m_isClickLeft && _firstClick)
        {
            //Debug.Log($"<color=blue> " + transform.rotation + " </color>");
            InputPlayer.Instance.m_GetMousePositionWorld();
            _posMouseScreen = InputPlayer.Instance.m_posSourisWorld;

            Vector3 LocalVectorScale = _posMouseScreen - _posSquare;
            transform.localScale = LocalVectorScale;

            DetectTank();

        }
    }

    private void CheckClickStop()
    {
        if(!m_isClickLeft && _firstClick) {
            {

                //Debug.Log($"<color=red> Stop Detection Zone </color>");
                _firstClick = false;
                transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                Vector3 LoacalVectorScale = new Vector3(0,0,0);
                transform.localScale = LoacalVectorScale;
            } }
    }

    private void DetectTank()
    {

        ContactFilter2D contactfliter = new ContactFilter2D();
        contactfliter.SetLayerMask(layerMask);
        
        

        Vector2 Size = new Vector2 (Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        Vector2 CenterPoint = new Vector2(_posSquare.x + (transform.localScale.x/2), _posSquare.y + (transform.localScale.y/2));

        

        Physics2D.OverlapBox(CenterPoint, Size, 0, contactfliter, m_collider2DsTank);

        for (int i = 0; i < m_collider2DsTank.Count; i++)
        {
            if(!m_collider2DsTank[i].CompareTag("Player"))
                _collider2DsTankDelete.Add(m_collider2DsTank[i]);
        }
        foreach (var tank in _collider2DsTankDelete)
        {
            m_collider2DsTank.Remove(tank);
        }
        _collider2DsTankDelete.Clear();

        foreach (Collider2D collider in m_collider2DsTank)
        {
            if (!_collider2DsTankCopy.Contains(collider))
            {
                
                _collider2DsTankCopy.Add(collider);
                collider.gameObject.GetComponent<SpriteRenderer>().material = _materialOutline;
            }


        }
        foreach (Collider2D collider in _collider2DsTankCopy)
        {
            if (!m_collider2DsTank.Contains(collider))
            {
                _collider2DsTankDelete.Add(collider);
                collider.gameObject.GetComponent<SpriteRenderer>().material = _materialNormal;
            }
        }

        foreach(Collider2D collider in _collider2DsTankDelete)
        {
            _collider2DsTankCopy.Remove(collider);
        }

        _collider2DsTankDelete.Clear();


        
    }
}
