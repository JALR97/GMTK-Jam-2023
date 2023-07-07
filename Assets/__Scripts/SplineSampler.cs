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

    [SerializeField] private int _splineIndex;
    [Range(0f, 1f)]
    [SerializeField] private float _time;
    [SerializeField] private float _width;
    //  [[ set in Start() ]] 


    //**    ---Variables---    **//
    //  [[ balance control ]] 

    //  [[ internal work ]] 
    private float3 position;
    private float3 p1;
    private float3 p2;
    private float3 tangent;
    private float3 upVector;

    //**    ---Properties---    **//


    //**    ---Functions---    **//
    private void Update() {
        _splineContainer.Evaluate(_splineIndex, _time, out position, out tangent, out upVector);

        float3 right = Vector3.Cross(tangent, upVector).normalized;
        p1 = position + (right * _width);
        p2 = position + (-right * _width);
    }
    
    //Debug
    private void OnDrawGizmos() {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0, p1, Quaternion.identity, 1f, EventType.Repaint);
        Handles.SphereHandleCap(0, p2, Quaternion.identity, 1f, EventType.Repaint);
    }
}
