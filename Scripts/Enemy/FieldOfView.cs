using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float viewRadius, meshResolution;
    float viewAngle=360;
    [SerializeField] MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public List<Transform> listOfTargets = new List<Transform>();
    private void Start()
    {

        viewMesh = new Mesh();
        viewMesh.name = "ViewMesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate()
    {
        // do not draw field of view for dead characters
        
        if (GetComponent<Health>() && GetComponent<Health>().IsDead())
        {
            viewMeshFilter.mesh = null;
            viewRadius = 0;
            viewAngle = 0;
            return; }
        DrawFieldOfView();
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // used to draw field of view
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float length, angle;
        public ViewCastInfo(bool _hit, Vector3 _point, float _length, float _angle)
        {
            hit = _hit;
            point = _point;
            length = _length;
            angle = _angle;
        }
    }
}


