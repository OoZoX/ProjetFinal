using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Camera _Camera;


    public Vector3 m_posSouri;

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
        GetMousePosition();
        GetKeyBoard();
    }

    private void GetMousePosition()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_posSouri = _Camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Debug.Log($"<color=yellow>" + m_posSouri.x + "</color> <color=blue>" + m_posSouri.y + "</color>");
        }
    }

    private void GetKeyBoard()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            MouvementCamera.Instance.m_depCamHaut = true;
        }
        else
            MouvementCamera.Instance.m_depCamHaut = false;

        if (Input.GetKey(KeyCode.S))
        {
            MouvementCamera.Instance.m_depCamBas = true;
        }
        else
            MouvementCamera.Instance.m_depCamBas = false;

        if (Input.GetKey(KeyCode.Q))
        {
            MouvementCamera.Instance.m_depCamGauche = true;
        }
        else
            MouvementCamera.Instance.m_depCamGauche = false;

        if (Input.GetKey(KeyCode.D))
        {
            MouvementCamera.Instance.m_depCamDroite = true;
        }
        else
            MouvementCamera.Instance.m_depCamGauche = false;
    }
}
