using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Camera _Camera;
    private Vector3 _Pos_souri;
    void Start()
    {
        _Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _Pos_souri = _Camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        Debug.Log($"<color=yellow>" + _Pos_souri.x + "</color> <color=blue>" + _Pos_souri.y + "</color>");
    }
}
