using Unity.VisualScripting;
using UnityEngine;
    
public class PlayerInput : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform SelectionBox;
    [SerializeField] private LayerMask UnitLayers;
    [SerializeField] private LayerMask MapLayers;

    private Vector2 startMousePosition;
    
    private void Update() {
        SelectionInputs();
        CommandInputs();
    }
    
    private void SelectionInputs() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(true);
            startMousePosition = Input.mousePosition;
        }else if (Input.GetKey(KeyCode.Mouse0)) {
            ResizeSelectionBox();
        }else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);
        }
    }
    
    private void ResizeSelectionBox() {
        float width = Input.mousePosition.x - startMousePosition.x;
        float height = Input.mousePosition.y - startMousePosition.y;

        SelectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

        for (int i = 0; i < GameManager.Instance.AvailableUnits.Count; i++) {
            if (UnitInSelection(_camera.WorldToScreenPoint(GameManager.Instance.AvailableUnits[i].Position()), bounds)) {
                GameManager.Instance.Select(GameManager.Instance.AvailableUnits[i]);
            }
            else {
                GameManager.Instance.Deselect(GameManager.Instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitInSelection(Vector2 pos, Bounds bounds) {
        return pos.x > bounds.min.x && pos.x < bounds.max.x && pos.y > bounds.min.y && pos.y < bounds.max.y;
    }

    private void CommandInputs() {
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Ray TargetPosition = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(TargetPosition, out var hitInfo)) {
                GameManager.Instance.MoveCommand(hitInfo.point);
            }
            Debug.Log(hitInfo;
        }
    }
}
