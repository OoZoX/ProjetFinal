using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGrid : MonoBehaviour
{
    [SerializeField]
    MyGameValue _gameValue;

    private void Awake()
    {
        _gameValue.m_grid = transform.gameObject;
    }


}
