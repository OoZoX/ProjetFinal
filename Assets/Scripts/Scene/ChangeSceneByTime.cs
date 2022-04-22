using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneByTime : MonoBehaviour
{
    private float time = 0;

    public float m_maxTime = 5.0f;
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if(time >= m_maxTime)
        {
            LoadScene.Instance.LoadMenu();
        }
    }
}
