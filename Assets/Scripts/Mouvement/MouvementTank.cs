using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MouvementTank : MonoBehaviour
{

    [SerializeField]
    GameParameters _gameParameters;
 
    private Vector3 _cible;

    public static MouvementTank Instance;


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

    
    void Update()
    {
        if (InputPlayer.Instance.m_clickMouseRight)
        {
            InputPlayer.Instance.m_GetMousePositionWorld();
            _cible = InputPlayer.Instance.m_posSourisWorld;
            Mouvement();
            
        }
    }



    private void Mouvement()
    {

        Cell CellCible = ManagerGraph.Instance.m_GetCellFromPosWorld(_cible);
        ManagerGraph.Instance.m_StartParcourChemin(CellCible);
        GridDebug.Instance.DrawFlowField();
        List<Collider2D> Tanks = SelectionTank.Instance.m_collider2DsTank;
        foreach (Collider2D tank in Tanks)
        {
            tank.GetComponent<PlayerTank>().m_startDep = true;
        }
        
    }



}
