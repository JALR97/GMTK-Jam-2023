using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

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
    [SerializeField] private int resolution;
    
    //  [[ internal work ]]
    private List<Vector3> _vertsP1;
    private List<Vector3> _vertsP2;
    
    //**    ---Properties---    **//

    
    //**    ---Functions---    **//
    private void OnEnable() {
        Spline.Changed += OnSplineChanged;
        GetVerts();
    }
    private void OnDisable() {
        Spline.Changed -= OnSplineChanged;
    }
    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3) {
        Rebuild();
    }
    
    private void Update() {
        GetVerts();
        BuildMesh();
    }
    private void Rebuild() {
        GetVerts();
        BuildMesh();
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

    private void BuildMesh() {
        Mesh m = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
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
                
                verts.AddRange(new List<Vector3>{ p1, p2, p3, p4});
                tris.AddRange(new List<int>{ t1, t2, t3, t4, t5, t6});
            }
        }
        m.SetVertices(verts);
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
