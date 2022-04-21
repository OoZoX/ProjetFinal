using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public bool m_playerInRange = false;
    public Vector3 m_posPlayer;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_playerInRange = true;
            m_posPlayer = collision.transform.position;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_playerInRange=false;
    }
}
