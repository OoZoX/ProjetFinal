using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ManagerGraph : MonoBehaviour
{
    [SerializeField]
    MyGameValue _gameValue;

    [SerializeField]
    LayerMask _layerMask;

    public List<Collider2D> m_tileCollider2D = new List<Collider2D>();

    public static ManagerGraph Instance;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_GetTileMapCollider();
        Debug.Log(_gameValue.m_grid.GetComponent<Tilemap>().size);
        Debug.Log(_gameValue.m_grid.GetComponent<Tilemap>().transform.position) ;
    }

    // Update is called once per frame

    void Update()
    {
        
    }


    public void m_GetTileMapCollider()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_layerMask);

        _gameValue.m_grid.GetComponent<TilemapCollider2D>().OverlapCollider(contactFilter, m_tileCollider2D);
        
    }
}
