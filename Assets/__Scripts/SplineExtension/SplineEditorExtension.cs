using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace UnityEditor.Splines
{
    public class SplineEditorExtension : MonoBehaviour
    {
        //**    ---Components---    **//
        //  [[ set in editor ]] 
    
        //  [[ set in Start() ]] 
    
    
        //**    ---Variables---    **//
        //  [[ balance control ]] 
    
        //  [[ internal work ]] 
        public struct SelectedSplineInfo {
            public Object target;
            public int targetIndex;
            public int knotIndex;

            public SelectedSplineInfo(Object obj, int index, int knot) {
                target = obj;
                targetIndex = index;
                knotIndex = knot;
            }
        }
        //**    ---Properties---    **//
    
    
        //**    ---Functions---    **//
        public static List<SelectedSplineInfo> GetSelection() {
            //Get internal struct data
            List<SelectableSplineElement> elements = SplineSelection.selection;
            
            //make new public struct data
            List<SelectedSplineInfo> infos = new List<SelectedSplineInfo>();
            
            //convert internal to public
            foreach (var element in elements) {
                infos.Add(new SelectedSplineInfo(element.target, element.targetIndex, element.knotIndex));
            }

            return infos;
        }
        
        public static bool HasSelection() {
            return SplineSelection.HasActiveSplineSelection();
        }
    }
}
