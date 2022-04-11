using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TypeCase
{
    VIDE,
    SOL,
    COLLIDER
}
public struct Case
{
    
    public Vector3 m_pos { get; set; }
    public TypeCase m_typeCase { get; set; }
    public Collider2D m_collider { get; set; }

}
public class ManagerGraph : MonoBehaviour
{
    [SerializeField]
    GameObjectVariable  GameObjectTileMap;
    [SerializeField]
    GameObjectVariable GameObjectScanMap;

    [SerializeField]
    LayerMask _layerMask;

    public float m_positionStart_X;
    public float m_positionStart_Y;
    private Vector3 _sizeMap;

    [SerializeField]
    public Case[,] m_listCaseMap;

    public static ManagerGraph Instance;

    
    void Awake()
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
        _sizeMap = GameObjectTileMap.m_value.GetComponent<Tilemap>().size;
        m_listCaseMap = new Case[(int)_sizeMap.x, (int)_sizeMap.y];

        m_ScanMapCollider();
        Debug.Log("fini");
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Scan la map pour crer un tableau [,] de toutes les cases
    /// </summary>
    public void m_ScanMapCollider()
    {
        
        Debug.Log(_sizeMap);
        for (int i = 0; i < _sizeMap.x; i++)
        {
            for (int u = 0; u < _sizeMap.y; u++)
            {
                Vector3 NewPos = new Vector3(i + m_positionStart_X, u + m_positionStart_Y, 0);
                GameObjectScanMap.m_value.transform.position = NewPos;

                RaycastHit2D raycast = Physics2D.Raycast(GameObjectScanMap.m_value.transform.position, Vector2.right, 0.1f, _layerMask);

                Case CaseTemp = new Case();


                if (raycast.collider)
                {
                    if (raycast.collider.isTrigger)
                    {
                        CaseTemp.m_pos = new Vector3(i + _sizeMap.x, u + _sizeMap.y, 0);
                        CaseTemp.m_typeCase = TypeCase.SOL;

                    }
                    else
                    {
                        CaseTemp.m_pos = new Vector3(i + _sizeMap.x, u + _sizeMap.y, 0);
                        CaseTemp.m_typeCase = TypeCase.COLLIDER;
                        CaseTemp.m_collider = raycast.collider;
                    }
                    
                }
                
                else
                {
                    CaseTemp.m_pos = new Vector3(i + _sizeMap.x, u + _sizeMap.y, 0);
                    CaseTemp.m_typeCase = TypeCase.VIDE;
                }


                m_listCaseMap[i,u] = CaseTemp;
                
            }
        }
        




        
    }

    
}
