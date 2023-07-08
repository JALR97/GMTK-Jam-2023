using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Intersections", menuName = "ScriptableObjects/Map Intersections", order = 1)]
public class MapIntersections_SO : ScriptableObject
{
    public Intersection[] intersections;
    
}
