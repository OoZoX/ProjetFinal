using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField]
    GameValue _gameStats;
    private void Awake()
    {
        _gameStats.m_camera = transform.gameObject;
    }

}
