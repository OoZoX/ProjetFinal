using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FovSetActive : MonoBehaviour
{
    List<Collider2D> _colliderList;
    List<Collider2D> _colliderVisible;
    List<Collider2D> _colliderToDelete;
    ContactFilter2D _contactFilter;

    [SerializeField]
    LayerMask _layerMask;

    void Start()
    {
        _contactFilter.SetLayerMask(_layerMask);
        _colliderVisible = new List<Collider2D>();
        _colliderToDelete = new List<Collider2D>();
        _colliderList = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _colliderList.Clear();
        Physics2D.OverlapCircle(transform.position, transform.GetComponent<Light2D>().pointLightOuterRadius,_contactFilter, _colliderList);
        foreach(Collider2D collider in _colliderList)
        {
            if (collider.gameObject.CompareTag("Ennemy"))
            {
                collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                _colliderVisible.Add(collider);
            }
        }

        foreach(Collider2D collider in _colliderVisible)
        {
            if (!_colliderList.Contains(collider))
            {
                collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                _colliderVisible.Remove(collider);
            }
        }
    }
}
