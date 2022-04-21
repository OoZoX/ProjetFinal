using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBar : MonoBehaviour
{
    public Image BarCaptur;
    float lerpspeed;

    void Start()
    {
    }

    void Update()
    {
        lerpspeed = 3.0f * Time.deltaTime;
        BarCapturFiller();
    }

    void BarCapturFiller()
    {
        BarCaptur.fillAmount = Mathf.Lerp(BarCaptur.fillAmount, GetComponent<CaptureZone>().CaptureActuelle / GetComponent<CaptureZone>().CaptureMax, lerpspeed);
    }

}
