using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public float maxSpeed = 5;
    public float size = .05f;

    void Start()
    {
        
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;
        move += transform.forward * (Input.GetKey(KeyCode.W) ? 1 : 0);
        move -= transform.forward * (Input.GetKey(KeyCode.S) ? 1 : 0);
        move -= transform.right * (Input.GetKey(KeyCode.A) ? 1 : 0);
        move += transform.right * (Input.GetKey(KeyCode.D) ? 1 : 0);
        move += Vector3.up * (Input.GetKey(KeyCode.Space) ? 1 : 0);
        move -= Vector3.up * (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);

        velocity += move.normalized * Time.deltaTime;
        velocity *= Mathf.Pow(0.5f, Time.deltaTime);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        position += velocity * Time.deltaTime;
    }
}
