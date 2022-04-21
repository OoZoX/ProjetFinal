using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Camera _Camera;
    [SerializeField]
    private LayerMask _layerRayCastTank;
    [SerializeField]
    //private string _tagTankAllie;


    public bool m_touchTankAllie = false;
    public bool m_clickMouseLeft = false;
    public bool m_clickMouseRight = false;
    public bool m_KeyboardN = false;

    public Vector3 m_posSourisWorld;
    public Vector3 m_posMouseScreen;

    public static InputPlayer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
            
    }
    void Start()
    {
        _Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetClickMouse();
        GetKeyBoard();
        GetKeyN();
    }



    private void RayCastClick()
    {
        m_GetMousePositionWorld();
        RaycastHit2D raycast = Physics2D.Raycast(m_posSourisWorld, Vector2.right, 0.1f, _layerRayCastTank);
        if(raycast.collider != null)
        {
            if (raycast.collider.gameObject.CompareTag("Player"))
                m_touchTankAllie = true;
        }
        
        else
            m_touchTankAllie = false;
    }

    private void GetClickMouse()
    {
        m_clickMouseLeft = Input.GetMouseButton(0);
        m_clickMouseRight = Input.GetMouseButton(1);
        if (m_clickMouseRight)
            RayCastClick();
        else 
            m_touchTankAllie = false;
         
    }
    public void m_GetMousePositionWorld()
    {
        m_posSourisWorld = _Camera.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"<color=yellow>" + m_posSourisWorld.x + "</color> <color=blue>" + m_posSourisWorld.y + "</color>");
    }
    public void GetMousePositionScreen()
    {
        m_posMouseScreen = Input.mousePosition;
        //Debug.Log($"<color=yellow>" + m_posSourisScreen.x + "</color> <color=blue>" + m_posSourisScreen.y + "</color>");
    }
    public void GetKeyN()
    {
        m_KeyboardN = Input.GetKey(KeyCode.N);
    }
    private void GetKeyBoard()
    {
        MouvementCamera.Instance.m_depCamHaut = Input.GetKey(KeyCode.Z);
        MouvementCamera.Instance.m_depCamBas = Input.GetKey(KeyCode.S);
        MouvementCamera.Instance.m_depCamGauche = Input.GetKey(KeyCode.Q);
        MouvementCamera.Instance.m_depCamDroite = Input.GetKey(KeyCode.D);
    }
}
