using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MyGameValue : ScriptableObject
{
    [Header("GameObject")]

    [Tooltip("GameObject camera a move")]
    public GameObject m_camera;

    [Tooltip("Grid Collider")]
    public GameObject m_grid;

    

}
