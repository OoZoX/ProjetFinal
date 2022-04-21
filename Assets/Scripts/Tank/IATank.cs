using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IATank : Tank
{
    //protected bool EnnemyInRange;
    [SerializeField] public DetectionZone m_detectionZone;
    public LayerMask m_layerMaskPlayer;

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_detectionZone.m_playerInRange == true)
        {
           _turret.OrientationTurret(m_detectionZone.m_posPlayer);
            _turret.ThrowProjectile();

        }
    
    }

    //private void _RayCastPlayer()
    //{
    //    Vector2 direction = (m_detectionZone.m_posPlayer - _turret.transform.position).normalized;

    //    ContactFilter2D contactFilter = new ContactFilter2D();
    //    contactFilter.SetLayerMask(m_layerMaskPlayer);
    //    contactFilter.useTriggers = true;

    //    List<RaycastHit2D> raycast = new List<RaycastHit2D>();

    //    Physics2D.Raycast(transform.position, direction,  contactFilter, raycast);

    //    if(raycast.Count > 0)
    //    {
    //        foreach (RaycastHit2D raycastHit in raycast)
    //        {
    //            Debug.Log("collider");
    //            if (raycastHit.collider.gameObject.CompareTag("Player"))
    //            {
    //                Debug.Log("Tire projectile");
    //                _turret.ThrowProjectile();
    //            }

    //        }

    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Ray ray = new Ray();
    //    Vector2 direction = (m_detectionZone.m_posPlayer - _turret.transform.position).normalized;
    //    ray.direction = direction;
    //    ray.origin = _turret.transform.position;

    //    Gizmos.DrawRay(ray);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetHeal(collision);
        CapturingZone(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CapturingZone(collision);
        GetHeal(collision);
    }
}
