using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] public Level _Level;
    [SerializeField] public Slider _slider;


    private float CaptureBarValue;
    private float TotalCaptureValue;
    private float MaxCaptureValue;
    // Start is called before the first frame update
    void Start()
    {
        ActualizeCaptureBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActualizeInfo()
    {
        
    }

    private void ActualizeCaptureBar()
    {
        TotalCaptureValue = 0;
        MaxCaptureValue = 0;
        CaptureBarValue = 0;
        foreach (CaptureZone captureZone in _Level.ZoneCaptureList)
        {
            TotalCaptureValue = TotalCaptureValue + captureZone.CaptureActuelle;
            MaxCaptureValue = MaxCaptureValue = captureZone.CaptureMax;
        }
        CaptureBarValue = (float)TotalCaptureValue / (float)MaxCaptureValue;
        _slider.value = CaptureBarValue;    
    }
}
