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
            _cible = InputPlayer.Instance.m_posMouseScreen;
            Mouvement();
        }
    }

    private void Mouvement()
    {
        ManagerGraph.Instance.m_StartParcourChemin(_cible);
        
        foreach(var tank in SelectionTank.Instance.m_collider2DsTank)
        {
            TraceCheminFlowFild(ManagerGraph.Instance.m_listCaseMap);
        }


    }

    private void TraceCheminFlowFild(Case[,] AllCaseMap)
    {
        foreach (var tank in SelectionTank.Instance.m_collider2DsTank)
        {
            Vector2 posTank = new Vector2(
                                            (int)tank.transform.position.x + ManagerGraph.Instance.m_decalageX, 
                                            (int) tank.transform.position.y + ManagerGraph.Instance.m_decalageY
                                         );

            
            Case LastCase;

            tank.GetComponent<PlayerTank>().m_cheminDep.Add(AllCaseMap[(int)posTank.x, (int)posTank.y].m_pos);
            LastCase = AllCaseMap[(int)posTank.x, (int)posTank.y];
            int DistanceCible = LastCase.m_distance;



            
        }
    }

    //private IEnumerator CalculeChemin()
    //{
    //    while (DistanceCible > 0)
    //    {
    //        tank.GetComponent<PlayerTank>().m_cheminDep.Add(AllCaseMap[
    //                                                                    (int)LastCase.m_parentSearch.x,
    //                                                                    (int)LastCase.m_parentSearch.y
    //                                                                  ].m_pos);

    //        LastCase = AllCaseMap[
    //                                (int)LastCase.m_parentSearch.x,
    //                                (int)LastCase.m_parentSearch.y
    //                             ];

    //        DistanceCible = LastCase.m_distance;

    //    }
    //}


}
