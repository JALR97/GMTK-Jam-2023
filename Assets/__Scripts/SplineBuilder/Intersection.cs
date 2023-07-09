using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class Intersection 
{
    public List<Junction> junctions;
        
    public void AddJunction(int splineIndex, int knotIndex, Spline spline, BezierKnot knot) {
        if (junctions == null) {
            junctions = new List<Junction>();
        }
        junctions.Add(new Junction(splineIndex, knotIndex, spline, knot));
    }

    internal IEnumerable<Junction> GetJunctions() {
        return junctions;
    }
}

[Serializable]
public class Junction {
    public int splineIndex;
    public int knotIndex;
    public Spline spline;
    public BezierKnot knot;

    public Junction(int splineIndex, int knotIndex, Spline spline, BezierKnot bezierKnot) {
        this.splineIndex = splineIndex;
        this.knotIndex = knotIndex;
        this.spline = spline;
        this.knot = bezierKnot;
    }
}
[Serializable]
public class BakeData {
    private List<Intersection> intersections;
}
