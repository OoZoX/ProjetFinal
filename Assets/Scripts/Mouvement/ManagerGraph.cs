using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TypeCase
{
    VIDE,
    SOL,
    COLLIDER,
    HERBE,
    ROUTE,
    SABLE
}
public struct Cell
{
    
    public Vector3 m_posWorld { get; set; }
    public Vector2Int m_gridIndex { get; set; }
    public byte m_cost { get; set; }
    public ushort m_bestCost { get; set; }
    public TypeCase m_typeCase { get; set; }
    public Collider2D m_collider { get; set; }
    public int m_distance { get; set; }
    public Vector2 m_parentSearch { get; set; }

}

public class ManagerGraph : MonoBehaviour
{
    [SerializeField]
    GameObjectVariable  GameObjectTileMap;
    [SerializeField]
    GameObjectVariable GameObjectScanMap;

    [SerializeField]
    LayerMask _layerMask;



    private float m_cellDiameter;



    public Vector3Int m_sizeMap;
    public Vector2Int m_sizeGrid;
    public float m_cellRadius;



    public bool m_flowFild = true;

    public float m_positionStart_X;
    public float m_positionStart_Y;

    
    

    
    public Cell[,] m_tabCellMap;

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
        CreateGrid();
        m_ScanMapCollider();
    }

    private void CreateGrid()
    {
        m_sizeMap = GameObjectTileMap.m_value.GetComponent<Tilemap>().size;

        m_cellDiameter = m_cellRadius * 2f;
        m_tabCellMap = new Cell[m_sizeGrid.x, m_sizeGrid.y];
        for (int x = 0; x < m_sizeGrid.x; x++)
        {
            for (int y = 0; y < m_sizeGrid.y; y++)
            {
                //Possible beug world pos
                Vector3 WorldPos = new Vector3(m_cellDiameter * x + m_cellRadius - m_positionStart_X, m_cellDiameter * y + m_cellRadius - m_positionStart_Y);
                Cell cell = new Cell();

                cell.m_posWorld = WorldPos;
                cell.m_gridIndex = new Vector2Int(x, y);
                cell.m_cost = byte.MaxValue;
                m_tabCellMap[x, y] = cell;
                
            }
        }

    }

    /// <summary>
    /// Calcule le cout de chaque case
    /// </summary>
    public void m_ScanMapCollider()
    {
        
        
        for (int x = 0; x < m_sizeGrid.x; x++)
        {
            for (int y = 0; y < m_sizeGrid.y; y++)
            {
                Vector2 NewPos = new Vector2(x - m_positionStart_X, y - m_positionStart_Y);
                Cell CurrentCell = m_tabCellMap[x, y];
                Vector2 CellExtend = Vector3.one * m_cellRadius;
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(_layerMask);
                List<Collider2D> collidersList = new List<Collider2D>();
                Physics2D.OverlapBox(CurrentCell.m_posWorld, CellExtend, 0, contactFilter, collidersList) ;

                bool checkRoute = false;
                foreach (Collider2D collider in collidersList)
                {
                    if(!checkRoute && collider.gameObject.layer == 6)//herbe
                    {
                        CurrentCell.m_cost = 2;
                    } 
                    else if(!checkRoute && collider.gameObject.layer == 7) //sable
                    {
                        CurrentCell.m_cost = 3;
                    }
                    else if(collider.gameObject.layer == 10) //collider
                    {
                        CurrentCell.m_cost = byte.MaxValue;
                        break;
                    }
                    else if (collider.gameObject.layer == 8) // route
                    {
                        CurrentCell.m_cost = 1;
                        checkRoute = true;
                    }

                }

                m_tabCellMap[x,y] = CurrentCell;

                //Debug.Log($"<color=red>" + i + " and " + u + "</color>");
                
            }
        }
        
        
    }

    public void m_StartParcourChemin(Vector2 Cible)
    {
        if (m_flowFild)
        {
            FlowBirth.Instance.ParcourtCarte(Cible);
        }
    }


}
