using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameObjectVariable : MonoBehaviour
{

    [SerializeField]
    GameObjectVariable gameValue;



    private void Awake()
    {
        gameValue.m_value = gameObject;
    }


}
