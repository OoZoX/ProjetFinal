using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITank : MonoBehaviour
{
    [SerializeField] public Slider _slider;
    [SerializeField] public Tank _Tank;
    [SerializeField] public ParticleSystem _TankParticules;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        ParticulesOnMouvement(); 
    }



    protected void ActualizeHealthBar()
    {
        _slider.value = (float)_Tank._Health / (float)_Tank._MaxHealth;
        Debug.Log(_Tank._Health + "  " + _Tank._MaxHealth + "  " + _slider.value);
    }
    protected void ParticulesOnMouvement()
    {
        if (_Tank._TankBody.velocity.y == 0 && _Tank._TankBody.velocity.x == 0)
        {
            _TankParticules.enableEmission = false;
        }
        else
        {
            _TankParticules.enableEmission = true;
        }

    }

}
