using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Camera _Camera;


    public Vector3 m_posSourisWorld;
    public Vector3 m_posSourisScreen;

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
        GetMousePositionScreen();
        GetMousePositionWorld();
        GetKeyBoard();
    }

    private void GetMousePositionWorld()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_posSourisWorld = _Camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"<color=yellow>" + m_posSourisWorld.x + "</color> <color=blue>" + m_posSourisWorld.y + "</color>");
        }
    }
    private void GetMousePositionScreen()
    {
        m_posSourisScreen = Input.mousePosition;
        //Debug.Log($"<color=yellow>" + m_posSourisScreen.x + "</color> <color=blue>" + m_posSourisScreen.y + "</color>");
    }

    private void GetKeyBoard()
    {
        MouvementCamera.Instance.m_depCamHaut = Input.GetKey(KeyCode.Z);
        MouvementCamera.Instance.m_depCamBas = Input.GetKey(KeyCode.S);
        MouvementCamera.Instance.m_depCamGauche = Input.GetKey(KeyCode.Q);
        MouvementCamera.Instance.m_depCamDroite = Input.GetKey(KeyCode.D);
    }
}
