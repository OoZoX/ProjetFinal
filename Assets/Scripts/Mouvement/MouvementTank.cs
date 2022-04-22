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
        if (InputPlayer.Instance.m_clickMouseRight && SelectionTank.Instance.m_collider2DsTank.Count > 0)
        {
            InputPlayer.Instance.m_GetMousePositionWorld();
            _cible = InputPlayer.Instance.m_posSourisWorld;
            MoveAt(_cible, true);
        }
    }

    /// <summary>
    /// lancce algo dep, si typeTank == true -> player / si non ennemy
    /// </summary>
    /// <param name="Cible"></param>
    /// <param name="typeTank"></param>
    public void MoveAt(Vector3 Cible, bool typeTank)
    {

        if (typeTank)
        {
            ManagerGraph.Instance.RefreshGrid();
            Mouvement(Cible);

            List<Collider2D> Tanks = SelectionTank.Instance.m_collider2DsTank;
            foreach (Collider2D tank in Tanks)
            {
                tank.GetComponent<PlayerTank>().m_startDep = true;
            }
        }
        else
        {
            ManagerGraph.Instance.RefreshGrid();
            Mouvement(Cible);
        }
        
    }


    private void Mouvement(Vector3 Cible)
    {

        Cell CellCible = ManagerGraph.Instance.m_GetCellFromPosWorld(Cible);
        ManagerGraph.Instance.m_StartParcourChemin(CellCible);
        GridDebug.Instance.DrawFlowField();
        

        
    }



}
