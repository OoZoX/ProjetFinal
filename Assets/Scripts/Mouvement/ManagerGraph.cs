using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ManagerGraph : MonoBehaviour
{
    [SerializeField]
    GameObjectVariable GameObjectTileMap;
    [SerializeField]
    GameObjectVariable GameObjectScanMap;

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
        m_ScanMapCollider();
        Debug.Log(GameObjectTileMap.m_value.GetComponent<Tilemap>().size);
        Debug.Log(GameObjectTileMap.m_value.GetComponent<Tilemap>().transform.position) ;
    }

    // Update is called once per frame

    void Update()
    {
        
    }


    public void m_ScanMapCollider()
    {
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = _layerMask;
        List<RaycastHit2D> resultRayCast = new List<RaycastHit2D>();

        Physics2D.Raycast(GameObjectScanMap.m_value.transform.position, Vector2.right, contactFilter2D, resultRayCast);

        
    }

    
}
