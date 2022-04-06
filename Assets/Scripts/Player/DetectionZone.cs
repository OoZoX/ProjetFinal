using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    //[SerializeField]
    //private GameValue _gameValue;
    [SerializeField]
    LayerMask layerMask;

    public bool _firstClick = false;
    private Vector3 _posMouseScreen;
    private Vector3 _posSquare;
    private List<Collider2D> collider2DsTank = new List<Collider2D>();
    private List<Collider2D> collider2DsTankCopy = new List<Collider2D>();
    private List<Collider2D> collider2DsTankDelete = new List<Collider2D>();

    public bool m_isClickLeft = false;

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
        InputPlayer.Instance.GetClickMouse();
        m_isClickLeft = InputPlayer.Instance.m_clickMouseLeft;
    }

    private void CheckClickStart()
    {
        if (m_isClickLeft && !_firstClick)
        {
            Debug.Log($"<color=green> Start Detection Zone </color>");
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

                Debug.Log($"<color=red> Stop Detection Zone </color>");
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

        

        Physics2D.OverlapBox(CenterPoint, Size, 0, contactfliter, collider2DsTank);

        foreach (Collider2D collider in collider2DsTank)
        {
            if (!collider2DsTankCopy.Contains(collider))
            {
                Debug.Log("pas dedans");
                collider2DsTankCopy.Add(collider);
                collider.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineWidth", 0.03f);
            }


        }
        foreach (Collider2D collider in collider2DsTankCopy)
        {
            if (!collider2DsTank.Contains(collider))
            {
                collider2DsTankDelete.Add(collider);
                collider.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineWidth", 0.0f);
            }
        }

        foreach(Collider2D collider in collider2DsTankDelete)
        {
            collider2DsTankCopy.Remove(collider);
        }

        collider2DsTankDelete.Clear();


        
    }
}
