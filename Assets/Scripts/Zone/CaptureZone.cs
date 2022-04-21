using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] public GameObject _Fill;
    [SerializeField] public TextMesh _Text;
    [SerializeField] public SpriteRenderer _SpriteFill;

    public float CaptureMax = 100f;
    private float CaptureMin;
    public float CaptureActuelle = 0f;

    public float FillScale = 0f;
    public float TextScale = 0f;

    private Color _FillColor;
    private Color _TextColor;
    public enum ZoneState
    {
        Empty,
        CapturingAI,
        CapturedAI,
        CapturingPlayer,
        CapturedPlayer
    }
    public ZoneState _ZoneState;

    void Start()
    {
        _ZoneState = ZoneState.Empty;
        CaptureMin = -CaptureMax;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("CaptureActuelle " + CaptureActuelle);
        if(CaptureActuelle < 0 && CaptureActuelle > CaptureMin)
        {
            _ZoneState = ZoneState.CapturingAI;
        }else if(CaptureActuelle <= CaptureMin)
        {
            _ZoneState = ZoneState.CapturedAI;
            CaptureActuelle = CaptureMin;
        }else if(CaptureActuelle < CaptureMax && CaptureActuelle > 0)
        {
            _ZoneState = ZoneState.CapturingPlayer;
        }else if (CaptureActuelle >= CaptureMax)
        {
            _ZoneState = ZoneState.CapturedPlayer;
        }

        switch (_ZoneState)
        {
            case ZoneState.Empty:
                break;
            case ZoneState.CapturingAI:
                CaptureActuelle = (float)Math.Round(CaptureActuelle);
                _TextColor = new Color(1, 0.5f, 0, 0.75f); //orange
                _FillColor = new Color(0, 0, 0, 0.1f);//transparent
                break;
            case ZoneState.CapturedAI:
                CaptureActuelle = CaptureMin;
                _TextColor = new Color(1, 0, 0, 1); //red
                _FillColor = new Color(0, 0, 0, 0.5f); //black
                break;
            case ZoneState.CapturingPlayer:
                CaptureActuelle = (float)Math.Round(CaptureActuelle);
                _TextColor = new Color(0, 1, 0, 0.7f); //vert
                _FillColor = new Color(1, 1, 0, 0.2f);//jaune
                break;
            case ZoneState.CapturedPlayer:
                CaptureActuelle = CaptureMax;
                _TextColor = new Color(0, 1, 0, 1);//vert
                _FillColor = new Color(1, 1, 0, 0.5f); // jaune
                break;

        }

        FillScale = CaptureActuelle / 10;
        if (CaptureActuelle < 0)
        {
            FillScale = FillScale  -(FillScale * 2);
        }
        TextScale = FillScale / 30;

        _Fill.transform.localScale = new Vector3(FillScale, FillScale, 1);
        _Text.transform.localScale = new Vector3(TextScale, TextScale, 1);

        _SpriteFill.color = _FillColor;
        _Text.color = _TextColor;

        _Text.text = CaptureActuelle.ToString();
    }

}
