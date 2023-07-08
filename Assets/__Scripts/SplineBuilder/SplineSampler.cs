using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    //**    ---Components---    **//
    //  [[ set in editor ]] 
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _width;
    [SerializeField] private MeshFilter _meshFilter;

    //**    ---Variables---    **//
    //  [[ balance control ]] 
    public int MapId;
    [SerializeField] private int resolution;
    
    //  [[ internal work ]]
    private List<Vector3> _vertsP1;
    private List<Vector3> _vertsP2;
    [SerializeField]private List<Intersection> intersections = new List<Intersection>();
    List<Vector3> meshVerts = new List<Vector3>();
    List<int> tris = new List<int>();

    //**    ---Properties---    **//

    
    //**    ---Functions---    **//
    private void OnEnable() {
        Spline.Changed += OnSplineChanged;
        Rebuild();
    }
    private void OnDisable() {
        Spline.Changed -= OnSplineChanged;
    }
    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3) {
        Rebuild();
    }
   
    private void Rebuild() {
        GetVerts();
        BuildMesh();
    }

    public void AddIntersection(Intersection intersection) {
        intersections.Add(intersection);
        Rebuild();
    }
    public void ClearIntersections() {
        intersections = new List<Intersection>();
        Rebuild();
    }
    private void SampleSplineWidth(int splineIndex, float time, out Vector3 p1, out Vector3 p2) {
        float3 position;
        float3 tangent;
        float3 upVector;
        _splineContainer.Evaluate(splineIndex, time, out position, out tangent, out upVector);
        float3 right = Vector3.Cross(tangent, upVector).normalized;
        p1 = position + (right * _width);
        p2 = position + (-right * _width);
    }
    
    private void GetVerts() {
        _vertsP1 = new List<Vector3>();
        _vertsP2 = new List<Vector3>();
        Vector3 vert1;
        Vector3 vert2;
        float step = 1f / (float)resolution;

        for (int j = 0; j < _splineContainer.Splines.Count; j++) {
            
            for (int i = 0; i < resolution; i++) {
                float t = step * i;
                SampleSplineWidth(j, t, out vert1, out vert2);
                _vertsP1.Add(vert1);
                _vertsP2.Add(vert2);
            }
            SampleSplineWidth(j, 1f, out vert1, out vert2);
            _vertsP1.Add(vert1);
            _vertsP2.Add(vert2);
        }
    }

    private void ClearMesh() {
        meshVerts = new List<Vector3>();
        tris = new List<int>();
    }
    
    private void GetJunctionVerts() {
        for (int i = 0; i < intersections.Count; i++) {
            Intersection intersection = intersections[i];
            Vector3 center = new Vector3();
            int count = 0;
            List<Vector3> points = new List<Vector3>();
            foreach (var juntion in intersection.GetJunctions()) {
                int splineIndex = juntion.splineIndex;
                float t = juntion.knotIndex == 0 ? 0f : 1f; //max time is 1f, but knot index must be normalized
                SampleSplineWidth(splineIndex, t, out Vector3 p1, out Vector3 p2);
                
                points.Add(p1);
                points.Add(p2);
                center += p1;
                center += p2;
                count++;
            }

            center /= points.Count;
            
            points.Sort((x, y) => {
                Vector3 xDir = x - center;
                Vector3 yDir = y - center;

                float angleA = Vector3.SignedAngle(center.normalized, xDir.normalized, Vector3.up);
                float angleB = Vector3.SignedAngle(center.normalized, yDir.normalized, Vector3.up);

                if (angleA > angleB) {
                    return 1;
                }

                if (angleA < angleB) {
                    return -1;
                }
                else {
                    return 0;
                }
            });
            
            int pointsOffset = meshVerts.Count;
            for (int j = 1; j <= points.Count; j++) {
                meshVerts.Add(center);
                meshVerts.Add(points[j - 1]);
                if (j == points.Count) {
                    meshVerts.Add(points[0]);
                }
                else {
                    meshVerts.Add(points[j]);
                }

                tris.Add(pointsOffset + ((j - 1) * 3) + 0);
                tris.Add(pointsOffset + ((j - 1) * 3) + 1);
                tris.Add(pointsOffset + ((j - 1) * 3) + 2);
            }
        }
    }

    private void BuildMesh() {
        ClearMesh();
        Mesh m = new Mesh();
        int offset = 0;

        int length = _vertsP2.Count;

        for (int currentIndex = 0; currentIndex < _splineContainer.Splines.Count; currentIndex++) {
            int splineOffset = resolution * currentIndex;
            splineOffset += currentIndex;

            for (int currentPoint = 1; currentPoint < resolution + 1; currentPoint++) {
                int vertOffset = splineOffset + currentPoint;
                Vector3 p1 = _vertsP1[vertOffset - 1];
                Vector3 p2 = _vertsP2[vertOffset - 1];
                Vector3 p3 = _vertsP1[vertOffset]; 
                Vector3 p4 = _vertsP2[vertOffset];

                offset = 4 * resolution * currentIndex;
                offset += 4 * (currentPoint - 1);
                
                int t1 = offset + 0;
                int t2 = offset + 2;
                int t3 = offset + 3;
            
                int t4 = offset + 3;
                int t5 = offset + 1;
                int t6 = offset + 0;
                
                meshVerts.AddRange(new List<Vector3>{ p1, p2, p3, p4});
                tris.AddRange(new List<int>{ t1, t2, t3, t4, t5, t6});
            }
        }

        GetJunctionVerts();
        
        m.SetVertices(meshVerts);
        m.SetTriangles(tris, 0);
        _meshFilter.mesh = m;
    }
    //Debug
    /*private void OnDrawGizmos() {
        Handles.matrix = transform.localToWorldMatrix;
        foreach (var vert in _vertsP1) {
            Handles.SphereHandleCap(0, vert, Quaternion.identity, 1f, EventType.Repaint);
        }
        foreach (var vert in _vertsP2) {
            Handles.SphereHandleCap(0, vert, Quaternion.identity, 1f, EventType.Repaint);
        }
    }*/
}
