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
    public Vector3 m_posTab { get; set; }
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

    private Vector3 _sizeMap;

    public bool m_flowFild = true;

    public float m_positionStart_X;
    public float m_positionStart_Y;
    

    
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
        //Debug.Log(_sizeMap);


        m_ScanMapCollider();

        
    }

    public void m_StartParcourChemin(Vector2 Cible)
    {
        if (m_flowFild)
        {
            FlowBirth.Instance.ParcourtCarte(Cible);
        }
    }



    /// <summary>
    /// Scan la map pour crer un tableau [,] de toutes les cases
    /// </summary>
    public void m_ScanMapCollider()
    {
        
        
        for (int i = 0; i < _sizeMap.x; i++)
        {
            for (int u = 0; u < _sizeMap.y; u++)
            {
                Vector3 NewPos = new Vector3(i, u, 0);
                GameObjectScanMap.m_value.transform.position = NewPos;

                RaycastHit2D raycast = Physics2D.Raycast(GameObjectScanMap.m_value.transform.position, Vector2.right, 0.1f, _layerMask);

                Case CaseTemp = new Case();

                CaseTemp.m_pos = new Vector3(NewPos.x - m_positionStart_X, NewPos.y - m_positionStart_Y, 0);
                CaseTemp.m_posTab = NewPos;
                if (raycast.collider != null)
                {
                    
                    if (raycast.collider.isTrigger)
                    {
                        CaseTemp.m_typeCase = TypeCase.SOL;
                    }
                    else
                    {
                        CaseTemp.m_typeCase = TypeCase.COLLIDER;
                        CaseTemp.m_collider = raycast.collider;
                    }
                }
                else
                {
                    CaseTemp.m_typeCase = TypeCase.VIDE;
                }

                m_listCaseMap[(int) NewPos.x, (int) NewPos.y] = CaseTemp;

                //Debug.Log($"<color=red>" + i + " and " + u + "</color>");
                
            }
        }
        
        
    }

    
}
