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
            TraceChemin(ManagerGraph.Instance.m_listCaseMap);
        }


    }

    private void TraceChemin(Case[,] AllCaseMap)
    {

    }


}
