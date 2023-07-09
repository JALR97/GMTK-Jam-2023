using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamController : MonoBehaviour {
    private Vector2 direction;
    [SerializeField] private float speed;
    
    
    //**    ---Functions---    **//
    private void Update() {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize();
        if (direction.magnitude > 0) {
            transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
}
