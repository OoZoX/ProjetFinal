using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlowBirth : MonoBehaviour
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


    public static FlowBirth Instance;
    
    

    private bool _searchFinish = false;
    private HashSet<Vector4> m_listToCalculateCompare;
    private List<Vector4> m_listToCalculate;
    private List<Vector2> _listCalculateFinish;
    private Case[,] _listTempCaseMap;


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

    public void ParcourtCarte(Vector2 Cible)
    {
        m_listToCalculateCompare = new HashSet<Vector4>(new CaseCompareur());
        m_listToCalculate = new List<Vector4>();
        _listCalculateFinish = new List<Vector2>();
        _listTempCaseMap = ManagerGraph.Instance.m_listCaseMap;
        Vector2 CibleActu;

        Cible = new Vector2(
                                Cible.x - ManagerGraph.Instance.m_positionStart_X, 
                                Cible.y - ManagerGraph.Instance.m_positionStart_Y
                           );


        m_listToCalculate.Add(new Vector4(Cible.x, Cible.y, Cible.x, Cible.y));
        m_listToCalculateCompare.Add(new Vector4(Cible.x, Cible.y, Cible.x, Cible.y));
        _searchFinish = false;

        while (!_searchFinish)
        {
            if (m_listToCalculate.Count == 0)
                _searchFinish = true;

            //VerifyDoubles();

            if (m_listToCalculate.Count != 0)
            {
                if(_listCalculateFinish.Count == 0)
                {
                    Debug.Log($"<color=red>" + Cible + "</color>");
                    _listTempCaseMap[(int)Cible.x, (int)Cible.y].m_parentSearch = new Vector2((int)Cible.x, (int)Cible.y);
                    _listTempCaseMap[(int)Cible.x, (int)Cible.y].m_distance = 0;
                    
                    _listCalculateFinish.Add(new Vector2(m_listToCalculate[0].x, m_listToCalculate[0].y));
                }
                else
                {
                    CibleActu = new Vector2((int)m_listToCalculate[0].x, (int)m_listToCalculate[0].y);
                    _listTempCaseMap[(int)CibleActu.x, (int)CibleActu.y].m_parentSearch = new Vector2((int)m_listToCalculate[0].z, (int)m_listToCalculate[0].w);
                    _listTempCaseMap[(int)CibleActu.x, (int)CibleActu.y].m_distance = _listTempCaseMap[(int)m_listToCalculate[0].z, (int)m_listToCalculate[0].w].m_distance + 1;
                }


                for (int i = 0; i < 4; i++)
                {
                    Vector2 NewCible = new Vector2(0,0);

                    if (i == 0)
                    {
                        NewCible = new Vector2(m_listToCalculate[0].x + 1, m_listToCalculate[0].y);
                    }
                    else if (i == 1)
                    {
                        NewCible = new Vector2(m_listToCalculate[0].x - 1, m_listToCalculate[0].y);
                    }
                    else if (i == 2)
                    {
                        NewCible = new Vector2(m_listToCalculate[0].x, m_listToCalculate[0].y + 1);
                    }
                    else if(i == 3)
                    {
                        NewCible = new Vector2(m_listToCalculate[0].x, m_listToCalculate[0].y - 1);
                    }
                    
                    if (!_listCalculateFinish.Contains(NewCible))
                    {
                        if(_listTempCaseMap[(int)NewCible.x, (int)NewCible.y].m_typeCase == TypeCase.SOL)
                        {
                            Vector4 AddCible = new Vector4(NewCible.x, NewCible.y, m_listToCalculate[0].x, m_listToCalculate[0].y);

                            if (m_listToCalculateCompare.Add(AddCible))
                                m_listToCalculate.Add(AddCible);
                        }

                    }

                }
                m_listToCalculate.RemoveAt(0);

            }
        }

        //ManagerGraph.Instance.m_listCaseMap = _listTempCaseMap;

    }


    //private void VerifyDoubles()
    //{
    //    for (int i = 0; i < m_listToCalculate.Count; i++)
    //    {
    //        Vector2 testCase = new Vector2(m_listToCalculate[0].x, m_listToCalculate[0].y);
    //        if (_listCalculateFinish.Contains(testCase))
    //        {
    //            m_listToCalculate.Remove(m_listToCalculate[0]);
    //            m_listToCalculateCompare.Remove(m_listToCalculate[0]);
    //        }
    //        else
    //        {

    //            break;
    //        }
    //    }
    //}

}                                           
                                             