using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class GameValue : ScriptableObject
{
    [Header("GameObject")]

    [Tooltip("GameObject camera a move")]
    [SerializeField] public GameObject m_camera;
    

}
