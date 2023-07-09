using UnityEngine;


public interface IUnit {
    public void Command(Vector3 Target);
    public void Selected();
    public void Deselected();
    public Vector3 Position();
}
