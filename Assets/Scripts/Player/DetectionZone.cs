using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    //[SerializeField]
    //private GameValue _gameValue;

    public bool _firstClick = false;
    private Vector3 _posMouseScreen;
    private Vector3 _posSquare;

    public bool m_isClickLeft = false;

    private void Start()
    {
        transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Vector3 loacalVectorScale = new Vector3(0, 0, 0);
        transform.localScale = loacalVectorScale;

    }

    private void Update()
    {
        GetInput();
        CheckClickStart();
        UpdateSquare();
        CheckClickStop();
    }

    private void GetInput()
    {
        InputPlayer.Instance.GetClickMouse();
        m_isClickLeft = InputPlayer.Instance.m_clickMouseLeft;
    }

    private void CheckClickStart()
    {
        if (m_isClickLeft && !_firstClick)
        {
            Debug.Log($"<color=green> Start Detection Zone </color>");
            _firstClick = true;
         
            InputPlayer.Instance.m_GetMousePositionWorld();
            _posSquare = InputPlayer.Instance.m_posSourisWorld;
            transform.position = _posSquare;

            transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            
        }
    }

    private void UpdateSquare()
    {
        if(m_isClickLeft && _firstClick)
        {
            Debug.Log($"<color=blue> " + transform.rotation + " </color>");
            InputPlayer.Instance.m_GetMousePositionWorld();
            _posMouseScreen = InputPlayer.Instance.m_posSourisWorld;

            Vector3 LocalVectorScale = _posMouseScreen - _posSquare;
            transform.localScale = LocalVectorScale;

           

        }
    }

    private void CheckClickStop()
    {
        if(!m_isClickLeft && _firstClick) {
            {
                Debug.Log($"<color=red> Stop Detection Zone </color>");
                _firstClick = false;
                transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Vector3 LoacalVectorScale = new Vector3(0,0,0);
                transform.localScale = LoacalVectorScale;
            } }
    }
}
