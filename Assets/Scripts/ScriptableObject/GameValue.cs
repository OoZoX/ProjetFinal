using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class GameValue : ScriptableObject
{
    [Header("GameObject")]

    [Tooltip("GameObject camera a move")]
    public GameObject m_camera;

    [Tooltip("GameObject Carre detection")]
    public GameObject m_carreDetection;
    

}
