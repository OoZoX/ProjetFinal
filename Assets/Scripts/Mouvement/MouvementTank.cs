using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MouvementTank : MonoBehaviour
{

    [SerializeField]
    GameParameters _gameParameters;

 
    private Vector3 _cible;


    void Start()
    {
        
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
        Debug.Log("<color=green>" + _cible + "</color>");
        ManagerGraph.Instance.m_StartParcourChemin(_cible);

        TraceCheminFlowFild(ManagerGraph.Instance.m_tabCellMap);
    }

    private void TraceCheminFlowFild(Cell[,] AllCaseMap)
    {
        foreach (var tank in SelectionTank.Instance.m_collider2DsTank)
        {

            StartCoroutine(CalculeChemin(tank, AllCaseMap));

        }
    }

    private IEnumerator CalculeChemin(Collider2D tank, Cell[,] AllCaseMap)
    {
        Vector2 posTank = new Vector2(
                                            (int)tank.transform.position.x - ManagerGraph.Instance.m_positionStart_X,
                                            (int)tank.transform.position.y - ManagerGraph.Instance.m_positionStart_Y
                                         );


        Cell LastCase;

        tank.GetComponent<PlayerTank>().m_cheminDep.Add(AllCaseMap[(int)posTank.x, (int)posTank.y].m_posWorld);
        LastCase = AllCaseMap[(int)posTank.x, (int)posTank.y];
        int DistanceCible = LastCase.m_distance; 

        while (DistanceCible > 0)
        {
            tank.GetComponent<PlayerTank>().m_cheminDep.Add(AllCaseMap[
                                                                        (int)LastCase.m_parentSearch.x,
                                                                        (int)LastCase.m_parentSearch.y
                                                                      ].m_posWorld);

            LastCase = AllCaseMap[
                                    (int)LastCase.m_parentSearch.x,
                                    (int)LastCase.m_parentSearch.y
                                 ];

            DistanceCible = LastCase.m_distance;
            yield return null;
        }
    }


}
