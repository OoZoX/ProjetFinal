using UnityEngine;

namespace FogOfWar
{
    public class FieldOfView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MeshFilter _viewMeshFilter;

        [Header("F.O.V. parameters")]
        [Range(2, 360)]
        [Tooltip("Le nombre de rayons à tirer pour trouver les obstacles")]
        [SerializeField] private int _rayCount = 360;
        [Range(1, 10)]
        [Tooltip("Le nombre d'itérations pour trouver les bords des obstacles")]
        [SerializeField] private int _edgeResolveIterations = 6;
        [Range(0, 2)]
        [SerializeField] private float _edgeDstThreshold = .5f;
        [Range(0, 1)]
        [Tooltip("La distance en m de pénétration du champ de vision dans un obstacle")]
        [SerializeField] private float _maskCutawayDistance = .25f;

        private Mesh _viewMesh;
        private MeshRenderer _meshRenderer;
        private Transform _transform;
        private MaterialPropertyBlock _matPropBlock; 

        private const int _maxVertexCount = (360 + 1) * 3;
        private Vector2[] _viewPoints = new Vector2[_maxVertexCount];
        private Vector3[] _vertices = new Vector3[_maxVertexCount + 1];
        private Vector2[] _uvs = new Vector2[_maxVertexCount + 1];
        private int[] _triangles = new int[(_maxVertexCount + 1 - 2) * 3];

        private ContactFilter2D _filter = new ContactFilter2D();
        private RaycastHit2D[] _hitBuffer = new RaycastHit2D[1];

        private void Awake()
        {
            _transform = transform;
            _matPropBlock = new MaterialPropertyBlock();

            _viewMesh = new Mesh();
            _viewMesh.name = "View Mesh";
            _viewMeshFilter.mesh = _viewMesh;
            _meshRenderer = _viewMeshFilter.GetComponent<MeshRenderer>();

            _filter.SetLayerMask(10);
        }

        void LateUpdate()
        {
            DrawFieldOfView();
            UpdateShader();
        }

        private void DrawFieldOfView()
        {
            float stepAngleSize = 360 / _rayCount;
            ViewCastInfo oldViewCast = new ViewCastInfo();
            int viewPointIndex = 0;
            for (int i = 0; i <= _rayCount; i++)
            {
                float angle = -_transform.eulerAngles.z - 360 / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.hit.distance - newViewCast.hit.distance) > _edgeDstThreshold;
                    if (oldViewCast.hit.isHit != newViewCast.hit.isHit || (oldViewCast.hit.isHit && newViewCast.hit.isHit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.hitA.point != Vector2.zero)
                        {
                            if (edge.hitA.isHit)
                            {
                                _viewPoints[viewPointIndex++] = GetCutawayPoint(edge.hitA);
                            }
                            else
                            {
                                _viewPoints[viewPointIndex++] = edge.hitA.point;
                            }
                        }
                        if (edge.hitB.point != Vector2.zero)
                        {
                            if (edge.hitB.isHit)
                            {
                                _viewPoints[viewPointIndex++] = GetCutawayPoint(edge.hitB);
                            }
                            else
                            {
                                _viewPoints[viewPointIndex++] = edge.hitB.point;
                            }
                        }
                    }
                }

                if (newViewCast.hit.isHit)
                {
                    _viewPoints[viewPointIndex++] = GetCutawayPoint(newViewCast.hit);
                }
                else
                {
                    _viewPoints[viewPointIndex++] = newViewCast.hit.point;
                }
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPointIndex + 1;

            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] = Vector2.zero;
            }
            for (int i = 0; i < _uvs.Length; i++)
            {
                _uvs[i] = Vector2.zero;
            }
            for (int i = 0; i < _triangles.Length; i++)
            {
                _triangles[i] = 0;
            }

            _vertices[0] = Vector2.zero;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                _vertices[i + 1] = _transform.InverseTransformPoint(_viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    _triangles[i * 3] = 0;
                    _triangles[i * 3 + 1] = i + 1;
                    _triangles[i * 3 + 2] = i + 2;
                }

                _uvs[i] = new Vector2(0.5f + (_vertices[i].x) / (2 * 6),
                                      0.5f + (_vertices[i].y) / (2 * 6));
            }

            _uvs[vertexCount - 1] = new Vector2(0.5f + (_vertices[vertexCount - 1].x) / (2 * 6),
                                                0.5f + (_vertices[vertexCount - 1].y) / (2 * 6));

            _viewMesh.Clear(false);

            _viewMesh.vertices = _vertices;
            _viewMesh.uv = _uvs;
            _viewMesh.triangles = _triangles;
            _viewMesh.RecalculateNormals();
            _viewMesh.RecalculateBounds();
        }

        #region Shader

        private void UpdateShader()
        {
            _matPropBlock.Clear();
            _matPropBlock.SetVector("_Center", new Vector4(_transform.position.x, _transform.position.y, 0, 0));
            _matPropBlock.SetFloat("_Radius", 6);
            _meshRenderer.SetPropertyBlock(_matPropBlock);
        }

        #endregion

        #region Utils

        private ViewCastInfo ViewCast(float globalAngle)
        {
            float range = 6;

            Vector2 dir = DirFromAngle(globalAngle, true);
            int count = Physics2D.Raycast(_transform.position, dir, _filter, _hitBuffer, range);
            if (count > 0)
            {
                return new ViewCastInfo(new CastHit(true, _hitBuffer[0].point, _hitBuffer[0].normal, _hitBuffer[0].distance), globalAngle);
            }
            else
            {
                return new ViewCastInfo(new CastHit(false, (Vector2)_transform.position + dir * range, -dir, range), globalAngle);
            }
        }

        private Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += _transform.eulerAngles.z;
            }
            return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private Vector2 GetCutawayPoint(CastHit hit)
        {
            Vector2 dirFromOrigin = (hit.point - (Vector2)_transform.position).normalized;
            return Vector2.MoveTowards(_transform.position, hit.point + dirFromOrigin * _maskCutawayDistance, 6);
        }

        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            CastHit minHit = minViewCast.hit;
            CastHit maxHit = maxViewCast.hit;


            for (int i = 0; i < _edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.hit.distance - newViewCast.hit.distance) > _edgeDstThreshold;
                if (newViewCast.hit.isHit == minViewCast.hit.isHit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minHit = newViewCast.hit;
                }
                else
                {
                    maxAngle = angle;
                    maxHit = newViewCast.hit;
                }
            }

            return new EdgeInfo(minHit, maxHit);
        }

        #endregion

        #region Structs
        private struct CastHit
        {
            public bool isHit;
            public Vector2 point;
            public Vector2 normal;
            public float distance;

            public CastHit(bool isHit, Vector2 point, Vector2 normal, float distance)
            {
                this.isHit = isHit;
                this.point = point;
                this.normal = normal;
                this.distance = distance;
            }
        }

        private struct ViewCastInfo
        {
            public CastHit hit;
            public float angle;

            public ViewCastInfo(CastHit hit, float angle)
            {
                this.hit = hit;
                this.angle = angle;
            }
        }

        private struct EdgeInfo
        {
            public CastHit hitA;
            public CastHit hitB;

            public EdgeInfo(CastHit hitA, CastHit hitB)
            {
                this.hitA = hitA;
                this.hitB = hitB;
            }
        }

        #endregion
    }
}
