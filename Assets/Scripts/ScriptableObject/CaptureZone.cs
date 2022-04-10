using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] public GameObject _Fill;
    [SerializeField] public GameObject _Border;
    [SerializeField] public TextMesh _Text;
    //[SerializeField] public GameObject _Capture;
    public static float CaptureMax = 100f;
    public static float CaptureActuelle = 0f;
    public float MaxScale = 10f;
    public float Scale = 0f;

    void Start()
    {
        Scale = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(CaptureMax > CaptureActuelle)
        {
            Scale = CaptureActuelle / 10;
            Debug.Log(CaptureActuelle + " CaptureActuellettrunc ");
            _Text.text = CaptureActuelle.ToString();
            CaptureActuelle = (float)Math.Round(CaptureActuelle);

        }
        else
        {
            CaptureActuelle = CaptureMax;
            _Text.text = CaptureActuelle.ToString();
            //_Text.transform.localScale =new Vector3(0.4f,0.4f ,0.4f);
            _Text.color = Color.red;
            Scale = 10;
        }
        _Fill.transform.localScale = new Vector3(Scale, Scale, 1);
        //_Capture.color = new Color(_Capture.color.r, _Capture.color.g, _Capture.color.b, _Capture.color.a);

    }



}
