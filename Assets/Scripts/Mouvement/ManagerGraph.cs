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
    public int m_distance { get; set; }
    public Vector2 m_parentSearch { get; set; }
    public GridDirection m_bestDirection { get; set; }

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



    //public Vector3Int m_sizeMap;
    public Vector2Int m_sizeGrid;
    public float m_cellRadius;



    public bool m_flowFild = true;

    public float m_positionStart_X;
    public float m_positionStart_Y;




    public Cell m_cibleCell;
    public Cell[,] m_tabCellMap;


    public bool refresh = false;

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
        m_positionStart_X = GameObjectScanMap.m_value.transform.position.x;
        m_positionStart_Y = GameObjectScanMap.m_value.transform.position.y;
        CreateGrid();
        m_ScanMapCollider();
    }

    private void FixedUpdate()
    {
        if (refresh)
        {

            RefreshGrid();
        }
    }

    public void RefreshGrid()
    {
        CreateGrid();
        m_ScanMapCollider();
        GridDebug.Instance.m_gridSize = m_sizeGrid;
    }

    private void CreateGrid()
    {
        //m_sizeMap = GameObjectTileMap.m_value.GetComponent<Tilemap>().size;

        m_cellDiameter = m_cellRadius * 2f;
        Cell[,] NewTab = new Cell[m_sizeGrid.x, m_sizeGrid.y];
        m_tabCellMap = NewTab;
        for (int x = 0; x < m_sizeGrid.x; x++)
        {
            for (int y = 0; y < m_sizeGrid.y; y++)
            {
                //Possible beug world pos
                Vector3 WorldPos = new Vector3(m_cellDiameter * x + m_positionStart_X - 0.5f + m_cellRadius, m_cellDiameter * y + m_positionStart_Y - 0.5f + m_cellRadius);
                Cell cell = new Cell();

                cell.m_posWorld = WorldPos;
                cell.m_gridIndex = new Vector2Int(x, y);
                cell.m_cost = byte.MaxValue;
                cell.m_bestCost = ushort.MaxValue;
                cell.m_bestDirection = GridDirection.None;

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
                contactFilter.useTriggers = true;
                List<Collider2D> collidersList = new List<Collider2D>();
                Physics2D.OverlapBox(CurrentCell.m_posWorld, CellExtend, 0, contactFilter, collidersList) ;

                bool checkRoute = false;
                foreach (Collider2D collider in collidersList)
                {
                    if(!checkRoute && collider.gameObject.layer == 6)//herbe
                    {
                        CurrentCell.m_cost = 2;
                       // Debug.Log("herbe");
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

    public Cell m_GetCellFromPosWorld(Vector3 worldPos)
    {
        float percentX = (worldPos.x - m_positionStart_X +0.5f) / (m_sizeGrid.x * m_cellDiameter);
        float percentY = (worldPos.y - m_positionStart_Y +0.5f) / (m_sizeGrid.y * m_cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((m_sizeGrid.x) * percentX), 0, m_sizeGrid.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((m_sizeGrid.y) * percentY), 0, m_sizeGrid.y - 1);

        m_cibleCell = m_tabCellMap[x, y];
        return m_tabCellMap[x, y];
    }



    public void m_StartParcourChemin(Cell Cible)
    {
        if (m_flowFild)
        {
            AlgoFlowField.Instance.m_CalculCost(Cible);
            AlgoFlowField.Instance.m_CreateFlowField();
        }
    }


}
