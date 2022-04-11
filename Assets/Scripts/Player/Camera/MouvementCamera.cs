using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MouvementCamera : MonoBehaviour
{
    
    private int ScreenHeight = Screen.height; 
    private int ScreenWidth = Screen.width;
    private GameObject _camera;

    [SerializeField]
    GameObjectVariable GameObjectCamera;
    [SerializeField]
    private int _zoneDepCam = 20;
    [SerializeField]
    private float _vitesseDepCam = 1f;

    public bool m_depCamHaut = false;
    public bool m_depCamBas = false;
    public bool m_depCamDroite = false;
    public bool m_depCamGauche = false;

    public bool m_depCamSouriHaut = false;
    public bool m_depCamSouriBas = false;
    public bool m_depCamSouriDroite = false;
    public bool m_depCamSouriGauche = false;

    public static MouvementCamera Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        _camera = GameObjectCamera.m_value;
    }


    void Update()
    {
        CheckPosSourisForDepCam();
        MoveCam();
    }

    private void CheckPosSourisForDepCam()
    {
        InputPlayer.Instance.GetMousePositionScreen();
        Vector3 PosSouri = InputPlayer.Instance.m_posMouseScreen;

        if (PosSouri.y > ScreenHeight - _zoneDepCam)
            m_depCamSouriHaut = true;
        else
            m_depCamSouriHaut = false;

        if (PosSouri.y < 0 + _zoneDepCam)
            m_depCamSouriBas = true;
        else
            m_depCamSouriBas = false;

        if (PosSouri.x > ScreenWidth - _zoneDepCam)
            m_depCamSouriDroite = true;
        else
            m_depCamSouriDroite = false;

        if (PosSouri.x < 0 + _zoneDepCam)
            m_depCamSouriGauche = true;
        else
            m_depCamSouriGauche = false;
    }

    private void MoveCam()
    {
        Vector3 DepCam = new Vector3();

        if ((m_depCamSouriHaut || m_depCamHaut) &&
                (m_depCamDroite || m_depCamSouriDroite))
            DepCam = new Vector3(_vitesseDepCam, _vitesseDepCam, 0) * Time.deltaTime;

        else if ((m_depCamSouriHaut || m_depCamHaut) &&
                (m_depCamGauche || m_depCamSouriGauche))
            DepCam = new Vector3(-_vitesseDepCam, _vitesseDepCam, 0) * Time.deltaTime;

        else if ((m_depCamSouriBas || m_depCamBas) &&
                (m_depCamDroite || m_depCamSouriDroite))
            DepCam = new Vector3(_vitesseDepCam, -_vitesseDepCam, 0) * Time.deltaTime;

        else if ((m_depCamSouriBas || m_depCamBas) &&
                (m_depCamGauche || m_depCamSouriGauche))
            DepCam = new Vector3(-_vitesseDepCam, -_vitesseDepCam, 0) * Time.deltaTime;


        else if (m_depCamSouriHaut || m_depCamHaut)
            DepCam = new Vector3(0, _vitesseDepCam, 0)   * Time.deltaTime;
        else if (m_depCamSouriBas || m_depCamBas)
            DepCam = new Vector3(0, -_vitesseDepCam, 0)  * Time.deltaTime;
        else if (m_depCamSouriDroite || m_depCamDroite)
            DepCam = new Vector3(_vitesseDepCam, 0, 0)   * Time.deltaTime;
        else if (m_depCamSouriGauche || m_depCamGauche)
            DepCam = new Vector3(-_vitesseDepCam, 0, 0)  * Time.deltaTime;
        

        if(DepCam != new Vector3(0,0,0))
            _camera.transform.position += DepCam;

    }
}
