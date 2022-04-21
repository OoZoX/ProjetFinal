using UnityEditor;
using UnityEngine;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public bool displayGrid = false;

	public FlowFieldDisplayType curDisplayType;

	public Vector2Int m_gridSize;
	private float cellRadius;
	

	private Sprite[] ffIcons;

	public static GridDebug Instance;

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

    private void Start()
	{
		displayGrid = false;
		ffIcons = Resources.LoadAll<Sprite>("Sprites/FFicons");
		//curFlowField = newFlowField;
		cellRadius = ManagerGraph.Instance.m_cellRadius;
		m_gridSize = ManagerGraph.Instance.m_sizeGrid;
	}

	
	public void DrawFlowField()
	{
		cellRadius = ManagerGraph.Instance.m_cellRadius;
		m_gridSize = ManagerGraph.Instance.m_sizeGrid;
		ClearCellDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.AllIcons:
				DisplayAllCells();
				break;

			case FlowFieldDisplayType.DestinationIcon:
				DisplayDestinationCell();
				break;

			default:
				break;
		}
	}

	private void DisplayAllCells()
	{
		
		foreach (Cell curCell in ManagerGraph.Instance.m_tabCellMap)
		{
			DisplayCell(curCell);
		}
	}

	private void DisplayDestinationCell()
	{
		if (ManagerGraph.Instance.m_tabCellMap == null) { return; }
		DisplayCell(ManagerGraph.Instance.m_cibleCell);
	}

	private void DisplayCell(Cell cell)
	{
		GameObject iconGO = new GameObject();
		SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
		iconGO.transform.parent = transform;
		iconGO.transform.position = cell.m_posWorld;

		if (cell.m_cost == 0)
		{
			iconSR.sprite = ffIcons[3];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_cost == byte.MaxValue)
		{
			iconSR.sprite = ffIcons[2];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.North)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.South)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.East)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.West)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.NorthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.NorthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.SouthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.m_bestDirection == GridDirection.SouthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else
		{
			iconSR.sprite = ffIcons[0];
		}
	}

	public void ClearCellDisplay()
	{
		foreach (Transform t in transform)
		{
			Destroy(t.gameObject);
		}
	}
	
	private void OnDrawGizmos()
	{
		if (displayGrid && ManagerGraph.Instance.m_sizeGrid == m_gridSize)
		{

			DrawGrid(ManagerGraph.Instance.m_sizeGrid, Color.green, ManagerGraph.Instance.m_cellRadius);
		}
		
		

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:

				foreach (Cell curCell in ManagerGraph.Instance.m_tabCellMap)
				{
					Handles.Label(curCell.m_posWorld, curCell.m_cost.ToString(), style);
				}
				break;
				
			case FlowFieldDisplayType.IntegrationField:

				foreach (Cell curCell in ManagerGraph.Instance.m_tabCellMap)
				{
					Handles.Label(curCell.m_posWorld, curCell.m_bestCost.ToString(), style);
				}
				break;
				
			default:
				break;
		}
		
	}

	private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = new Vector3(ManagerGraph.Instance.m_tabCellMap[x,y].m_posWorld.x, ManagerGraph.Instance.m_tabCellMap[x, y].m_posWorld.y, 0);
				//Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius + ManagerGraph.Instance.m_positionStart_X, drawCellRadius * 2 * y + drawCellRadius + ManagerGraph.Instance.m_positionStart_Y,0 );
				Vector3 size = Vector3.one * drawCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}
