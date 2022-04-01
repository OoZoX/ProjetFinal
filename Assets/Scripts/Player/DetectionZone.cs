using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField]
    private GameValue _gameValue;


    public bool m_isClickLeft = false;

    public static DetectionZone Instance;

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
        
    }

    private void CheckClickStart()
    {
        if (m_isClickLeft)
        {

        }
    }
}
