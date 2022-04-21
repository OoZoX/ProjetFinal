using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class FovSetActive : MonoBehaviour
{
    List<Collider2D> _colliderList;
    List<Collider2D> _colliderVisible;

    ContactFilter2D _contactFilter;

    [SerializeField]
    LayerMask _layerMask;

    void Start()
    {
        _contactFilter.SetLayerMask(_layerMask);
        _colliderVisible = new List<Collider2D>();
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
                collider.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(0).gameObject.SetActive(true);
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                _colliderVisible.Add(collider);
            }
        }

        foreach(Collider2D collider in _colliderVisible)
        {
            if (!_colliderList.Contains(collider))
            {
                collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                collider.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(0).gameObject.SetActive(false);
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                collider.gameObject.GetComponent<IATank>()._UITank.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                _colliderVisible.Remove(collider);
            }
        }
    }
}
