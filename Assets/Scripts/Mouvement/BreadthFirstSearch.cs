using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct AlgoDep
{

}
public class BreadthFirstSearch : MonoBehaviour
{
    public static BreadthFirstSearch Instance;
    
    


    /// <summary>
    /// { { (position case), (dist player, index unique), (indexer V1, indexV2), (indexV3, indexV4)}, {...} }
    /// si voisin hors map index = -1 
    /// </summary>
    private List<List<Vector2>> AllCases;

    /// <summary>
    /// recupere l'index unique de la case par rapport a sa pos. Permet de recuperer les infos de la case avec AllCases[PosToIndex[x,y]]
    /// </summary>
    private int[,] PosToIndex;


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



}                                           
                                             