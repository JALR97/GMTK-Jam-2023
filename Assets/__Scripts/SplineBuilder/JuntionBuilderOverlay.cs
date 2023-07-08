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
    
    [EditorToolbarElement(Id, typeof(SceneView))]
    public class ClearJunctionsButton : EditorToolbarButton {
        public const string Id = "Clear Junctions Button";

        public ClearJunctionsButton() {
            this.text = "Clear ALL Junctions";
            this.clicked += OnClick;
        }

        void OnClick() {
            ClearJunctions();
        }
    }
    
    [EditorToolbarElement(Id, typeof(SceneView))]
    public class BakeIntersectionsButton : EditorToolbarButton {
        public const string Id = "Bake Intersections Button";

        public BakeIntersectionsButton() {
            this.text = "Bake";
            this.clicked += OnClick;
        }

        void OnClick() {
            Bake();
        }
    }
    
    [EditorToolbarElement(Id, typeof(SceneView))]
    public class LoadBakeButton : EditorToolbarButton {
        public const string Id = "Load Bake Button";

        public LoadBakeButton() {
            this.text = "Load Bake";
            this.clicked += OnClick;
        }

        void OnClick() {
            LoadBake();
        }
    }
    public JuntionBuilderOverlay() : 
        base(BuildJunctionButton.Id, ClearJunctionsButton.Id, BakeIntersectionsButton.Id, LoadBakeButton.Id) { }

    private static void ClearJunctions() {
        SplineContainer container = (SplineContainer)SplineEditorExtension.GetSelection()[0].target;
        container.GetComponent<SplineSampler>().ClearIntersections();
    }
    private static void Bake() {
        SplineContainer container = (SplineContainer)SplineEditorExtension.GetSelection()[0].target;
        container.GetComponent<SplineSampler>().BakeIntersections();
    }
    private static void LoadBake() {
        SplineContainer container = (SplineContainer)SplineEditorExtension.GetSelection()[0].target;
        container.GetComponent<SplineSampler>().LoadBake();
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

        Selection.activeObject.GetComponent<SplineSampler>().AddIntersection(intersection);
    }
    private void UpdateSelectionInfo() {
        List<SplineEditorExtension.SelectedSplineInfo> infos = SplineEditorExtension.GetSelection();
        foreach (var element in infos) {
            root.Add(new Label() { text = $"Spline {element.targetIndex}, Knot {element.knotIndex}"});
        }
    }
}
