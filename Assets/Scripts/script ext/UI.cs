using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] public Slider _slider;
    [SerializeField] public TextMeshProUGUI _TextTimer;
    [SerializeField] public TextMeshProUGUI _TextScore;


    private float CaptureBarValue;
    private float TotalCaptureValue;
    private float MaxCaptureValue;
    // Start is called before the first frame update
    void Start()
    {
        //ActualizeCaptureBar();
        GetCaptureMax();
        ActualizeTimer();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeCaptureBar();
        ActualizeTimer();
        ActualizeScore();
    }
    private void ActualizeScore()
    {
        _TextScore.text = Level.Instance.score.ToString();
        //Debug.Log(" Level.Instance.score.ToString() " + Level.Instance.score.ToString());
    }
    private void ActualizeTimer()
    {
        _TextTimer.text = Mathf.RoundToInt((float)Level.Instance._GameTimer.Elapsed.TotalSeconds).ToString();
    }
    private void GetCaptureMax()
    {
        MaxCaptureValue = 0;
        foreach (CaptureZone captureZone in Level.Instance.ZoneCaptureList)
        {
            MaxCaptureValue = MaxCaptureValue + captureZone.CaptureMax;
        }

    }
    private void ActualizeCaptureBar()
    {
        TotalCaptureValue = 0;
        //CaptureBarValue = 0;
        
        foreach (CaptureZone captureZone in Level.Instance.ZoneCaptureList)
        {
            TotalCaptureValue = TotalCaptureValue + captureZone.CaptureActuelle;
        }
        CaptureBarValue = (float)TotalCaptureValue / (float)MaxCaptureValue;

        if (CaptureBarValue != _slider.value)
        {
            _slider.value = CaptureBarValue;
            //Debug.Log(" Level.Instance.score.ToString() " + CaptureBarValue);
        }
        /*
        Debug.Log(" (float)TotalCaptureValue " + (float)TotalCaptureValue);
        Debug.Log(" (float)CaptureBarValue " + CaptureBarValue);
        Debug.Log(" MaxCaptureValue " + MaxCaptureValue);
        
        */
    }
}
