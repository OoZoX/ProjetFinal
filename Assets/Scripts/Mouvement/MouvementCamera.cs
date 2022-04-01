using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementCamera : MonoBehaviour
{
    private Camera _camera = Camera.main;
    private int ScreenHeight = Screen.height; 
    private int ScreenWidth = Screen.width;

    [SerializeField]
    private int ZoneDepCam = 20;
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
        
    }


    void Update()
    {
        CheckPosSourisForDepCam();
    }

    private void CheckPosSourisForDepCam()
    {
        Vector3 PosSouri = InputPlayer.Instance.m_posSouri;

        if (PosSouri.y > ScreenHeight - ZoneDepCam)
            m_depCamSouriHaut = true;
        else
            m_depCamHaut = false;

        if (PosSouri.y < ScreenHeight + ZoneDepCam)
            m_depCamSouriBas = true;
        else
            m_depCamSouriBas = false;

        if (PosSouri.x > ScreenWidth - ZoneDepCam)
            m_depCamDroite = true;
        else
            m_depCamDroite = false;

        if (PosSouri.x < ScreenWidth + ZoneDepCam)
            m_depCamGauche = true;
        else
            m_depCamDroite = false;
    }

    private void MoveCam()
    {
        Vector3 DepCam;
        if ((m_depCamSouriHaut || m_depCamHaut) && (m_depCamDroite || m_depCamSouriDroite))
        {
            // Il faut le normalize pour avoir une vitesse constante
            DepCam = new Vector3(_vitesseDepCam, _vitesseDepCam, 0);
            _camera.transform.position += DepCam;
        }
        else if (m_depCamSouriHaut || m_depCamHaut)
        {
            DepCam = new Vector3(0, _vitesseDepCam, 0);
            _camera.transform.position += DepCam;
        }
        
        
        else if (m_depCamBas || m_depCamSouriBas)
        {
            DepCam = new Vector3(0, - _vitesseDepCam, 0);
            _camera.transform.position += DepCam;
        }
    }
}
