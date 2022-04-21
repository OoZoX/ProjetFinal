using UnityEngine;

namespace FogOfWar
{
    public class FOWSetup : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _fogCamera;
        [SerializeField] private RectTransform _fogCanvas;

        private void Start()
        {
            _fogCamera.orthographicSize = _mainCamera.orthographicSize * _mainCamera.aspect;
            _fogCanvas.sizeDelta = new Vector2(_fogCamera.orthographicSize, _fogCamera.orthographicSize) * 2;
        }
    }
}