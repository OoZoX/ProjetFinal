using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlgoFlowField : MonoBehaviour
{
    private class CaseCompareur : IEqualityComparer<Vector4>
    {
        public bool Equals(Vector4 V1, Vector4 V2)
        {
            return V1.x == V2.x && V1.y == V2.y;
        }

        public int GetHashCode(Vector4 obj)
        {
            return new Vector2(obj.x, obj.y).GetHashCode();
        }

    }


    public static AlgoFlowField Instance;
    
    

    
    private HashSet<Vector4> m_listToCalculateCompare;
    private List<Vector4> m_listToCalculate;
    private List<Vector2> _listCalculateFinish;

    private Cell[,] _listTempCellMap;

    List<Cell> cellsToCheck;


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

    public void m_CalculCost(Cell CellCible)
    {
        _listTempCellMap = ManagerGraph.Instance.m_tabCellMap;
        CellCible.m_cost = 0;
        CellCible.m_bestCost = 0;
        _listTempCellMap[CellCible.m_gridIndex.x, CellCible.m_gridIndex.y].m_bestCost = 0;

        cellsToCheck = new List<Cell>();

        cellsToCheck.Add(CellCible);

        while (cellsToCheck.Count > 0)
        {
            Cell CurrentCell = cellsToCheck[0];
            cellsToCheck.RemoveAt(0);
            List<Cell> CurrentNeighbor = GetNeighbors(CurrentCell.m_gridIndex, GridDirection.CardinalDirections);
            

            foreach (Cell Neighbor in CurrentNeighbor)
            {
                if (Neighbor.m_cost == byte.MaxValue)
                    continue;
                if(Neighbor.m_cost + CurrentCell.m_bestCost < Neighbor.m_bestCost)
                {
                    _listTempCellMap[Neighbor.m_gridIndex.x, Neighbor.m_gridIndex.y].m_bestCost = (ushort)(Neighbor.m_cost + CurrentCell.m_bestCost);

                    cellsToCheck.Add(_listTempCellMap[Neighbor.m_gridIndex.x, Neighbor.m_gridIndex.y]);
                }
            }
        }

        ManagerGraph.Instance.m_tabCellMap = _listTempCellMap;

    }

    
    public void m_CreateFlowField()
    {
        foreach (Cell CurrentCell in _listTempCellMap)
        {
            List<Cell> CurrentNeighbors = GetNeighbors(CurrentCell.m_gridIndex, GridDirection.AllDirections);
            int BestCost = CurrentCell.m_bestCost;
            foreach(Cell currentNeighbor in CurrentNeighbors)
            {
                if(currentNeighbor.m_bestCost < BestCost)
                {
                    BestCost = currentNeighbor.m_bestCost;
                    _listTempCellMap[CurrentCell.m_gridIndex.x, CurrentCell.m_gridIndex.y].m_bestDirection = GridDirection.GetDirectionFromV2I(currentNeighbor.m_gridIndex - CurrentCell.m_gridIndex);

                }
            }
        }

        ManagerGraph.Instance.m_tabCellMap = _listTempCellMap;
    }



    private List<Cell> GetNeighbors(Vector2Int indexParent, List<GridDirection> directions)
    {
        List<Cell> neighbors = new List<Cell>();
        Cell OriginCell = _listTempCellMap[indexParent.x, indexParent.y];

        foreach (Vector2Int CurrentDirections in directions)
        {
            Cell NewNeightbor = GetCellPosInGrid(indexParent, CurrentDirections);
            if(NewNeightbor.m_gridIndex != OriginCell.m_gridIndex)
            {
                neighbors.Add(NewNeightbor);
            }
        }

        return neighbors;
    }

    private Cell GetCellPosInGrid(Vector2Int origin, Vector2Int direction)
    {
        Vector2Int finalPos = origin + direction;
        Vector2Int gridSize = ManagerGraph.Instance.m_sizeGrid;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
            return _listTempCellMap[origin.x, origin.y];
        
        else 
            return _listTempCellMap[finalPos.x, finalPos.y]; 
    }


}                                           
                                             