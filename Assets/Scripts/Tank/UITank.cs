using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITank : MonoBehaviour
{
    [SerializeField] public Slider _slider;
    [SerializeField] public Tank _Tank;
    [SerializeField] public ParticleSystem _TankParticules;

    private bool DeathTrigger;
    // Start is called before the first frame update
    void Start()
    {
        DeathTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        ActualizeHealthBar();
        ParticulesOnMouvement();
        CheckHealth();
    }

    public void CheckHealth()
    {
        if(_Tank._Health <= 0 && DeathTrigger == false)
        {
            StartCoroutine(_Tank.DeathExplosion()); 
            DeathTrigger = true;
        }
    }

    protected void ActualizeHealthBar()
    {
        _slider.value = (float)_Tank._Health / (float)_Tank._MaxHealth;
        transform.position = new Vector3(_Tank.gameObject.transform.position.x + 0.02f, _Tank.gameObject.transform.position.y + 0.8f, transform.position.z);
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
