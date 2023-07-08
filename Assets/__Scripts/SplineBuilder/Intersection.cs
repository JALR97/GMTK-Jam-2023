using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Intersection 
{
    public struct JunctionInfo {
        public int splineIndex;
        public int knotIndex;
        public Spline spline;
        public BezierKnot knot;

        public JunctionInfo(int splineIndex, int knotIndex, Spline spline, BezierKnot knot) {
            this.splineIndex = splineIndex;
            this.knotIndex = knotIndex;
            this.spline = spline;
            this.knot = knot;
        }
    }
    public List<JunctionInfo> junctions;
    public Junction_SO[] Junctions_SO;
        
    public void AddJunction(int splineIndex, int knotIndex, Spline spline, BezierKnot knot) {
        if (junctions == null) {
            junctions = new List<JunctionInfo>();
        }
        junctions.Add(new JunctionInfo(splineIndex, knotIndex, spline, knot));
    }

    internal IEnumerable<JunctionInfo> GetJunctions() {
        return junctions;
    }
    public void SaveJunctions_SO() {
        Junctions_SO = new Junction_SO[junctions.Count];
        for (int i = 0; i < junctions.Count; i++) {
            Junctions_SO[i] = ScriptableObject.CreateInstance<Junction_SO>();
            Junctions_SO[i].junction = junctions[i];
        }
    }
    public void LoadJunctions_SO() {
        junctions = new List<JunctionInfo>();
        foreach (var junction_SO in Junctions_SO) {
            junctions.Add(junction_SO.junction);
        }
    }
}

public class Junction_SO : ScriptableObject {
    public Intersection.JunctionInfo junction;
} 