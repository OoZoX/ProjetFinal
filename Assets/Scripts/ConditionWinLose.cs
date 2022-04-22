using UnityEngine;

public class ConditionWinLose : MonoBehaviour
{

    private void FixedUpdate()
    {
        if (Level.Instance._StateLevel == Level.StateLevel.Victory)
        {
            LoadScene.Instance.LoadOneScene("Win");
        }
        else if (Level.Instance._StateLevel == Level.StateLevel.Defeat)
        {
            LoadScene.Instance.LoadOneScene("Lose");
        }
    }
}
