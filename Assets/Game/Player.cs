using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector3 position = new Vector3(0, 20, 0);
    public Vector3 velocity = Vector3.zero;
    public float maxSpeed = 1;
    public float size = 0.01f;
    public float smoothSize;
    public float accelerationFactor = .02f;

    public float gravity = 0.01f;

    public float minSize = 1e-5f;
    public float maxSize = 0.1f;
    public SDF sdf;

    void Start()
    {
        smoothSize = size;
        sdf = new MengerSponge();
    }

    public void HandleMovement()
    {
        size *= Mathf.Pow(0.5f, Input.GetAxis("Mouse ScrollWheel"));
        size = Mathf.Clamp(size, minSize, maxSize);
        smoothSize = Mathf.Lerp(smoothSize, size, Time.deltaTime);

        Vector3 move = Vector3.zero;
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        move -= forward * (Input.GetKey(KeyCode.S) ? 1 : 0);
        move += forward * (Input.GetKey(KeyCode.W) ? 1 : 0);
        move -= right * (Input.GetKey(KeyCode.A) ? 1 : 0);
        move += right * (Input.GetKey(KeyCode.D) ? 1 : 0);
        move += Vector3.up * (Input.GetKey(KeyCode.Space) ? 1 : 0);
        move -= Vector3.up * (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);


        float sizeFactor = Mathf.Clamp01((smoothSize - minSize) / (maxSize - minSize));
        float scaledAcceleration = accelerationFactor * (Mathf.Lerp(0.2f, 1.0f, Mathf.Sqrt(sizeFactor)) + 0.1f);
        float scaledMaxSpeed = maxSpeed * (smoothSize / maxSize) * 10f; // tweak the 10f as needed

        velocity += move.normalized * scaledAcceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 1.5f); // smooth drag
        velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * 1.5f); // smooth drag
        //velocity = Vector3.ClampMagnitude(velocity, scaledMaxSpeed);
        Vector3 gravityVector = sdf.normalAt(position) * gravity;
        velocity -= gravityVector * Time.deltaTime;

        position += velocity * Time.deltaTime;
        if(!BallPhysics.IsValidPosition(position, size, sdf)){
            (position, velocity) = BallPhysics.ResolveCollision(ref position, ref velocity, size, sdf);
        }
    }

    

    void OnGUI()
    {   
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 10, 100, 20), $"Size: {size}", style);
        GUI.Label(new Rect(10, 30, 100, 20), $"Distance: {sdf.signedDistanceAt(position)}", style);
        GUI.Label(new Rect(10, 50, 100, 20), $"Velocity: {velocity.magnitude}", style);
    }
}
