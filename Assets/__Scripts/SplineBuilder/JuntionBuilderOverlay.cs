using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Splines;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Splines;

[Overlay(typeof(SceneView), "JunctionBuilder", true)]
public class JuntionBuilderOverlay : ToolbarOverlay {
    
    private VisualElement root;
 
    [EditorToolbarElement(Id, typeof(SceneView))]
    public class BuildJunctionButton : EditorToolbarButton {
        public const string Id = "Junction Button";

        public BuildJunctionButton() {
            this.text = "Build Junction";
            this.clicked += OnClick;
        }

        void OnClick() {
            BuildJunction();
        }
    }
       
    public JuntionBuilderOverlay() : base(BuildJunctionButton.Id) {
    }
    
    private static void BuildJunction() {
        List<SplineEditorExtension.SelectedSplineInfo> selection = SplineEditorExtension.GetSelection();
        Intersection intersection = new Intersection();
        foreach (var splineInfo in selection) {
            SplineContainer container = (SplineContainer)splineInfo.target;
            Spline spline = container.Splines[splineInfo.targetIndex];
            //Could have issues
            intersection.AddJunction(splineInfo.targetIndex, splineInfo.knotIndex, spline, spline.Knots.ToList()[splineInfo.knotIndex]);
        }

        Selection.activeObject.GetComponent<SplineSampler>().AddJunction(intersection);
    }
    private void UpdateSelectionInfo() {
        List<SplineEditorExtension.SelectedSplineInfo> infos = SplineEditorExtension.GetSelection();
        foreach (var element in infos) {
            root.Add(new Label() { text = $"Spline {element.targetIndex}, Knot {element.knotIndex}"});
        }
    }
}
