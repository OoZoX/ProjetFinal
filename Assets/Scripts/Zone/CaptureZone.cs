using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] public GameObject _Fill;
    [SerializeField] public TextMesh _Text;
    public float CaptureMax = 100f;
    public float CaptureActuelle = 0f;
    public float Scale = 0f;
    public static CaptureZone Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Scale = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(CaptureMax > CaptureActuelle)
        {
            if(CaptureActuelle < 0)
            {
                CaptureActuelle = 0;
            }
            Scale = CaptureActuelle / 10;
            _Text.text = CaptureActuelle.ToString();
            CaptureActuelle = (float)Math.Round(CaptureActuelle);
            _Text.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            _Text.color = new Color(0, 0, 0, 0.7f);
        }
        else
        {
            CaptureActuelle = CaptureMax;
            _Text.text = CaptureActuelle.ToString();
            _Text.transform.localScale = new Vector3(0.3f,0.3f ,0.3f);
            _Text.color = new Color(1, 0, 0, 1); 
        }
        _Fill.transform.localScale = new Vector3(Scale, Scale, 1);
    }
}
