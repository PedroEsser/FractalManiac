using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public float maxSpeed = 1;
    public float size = 0.01f;
    public float smoothSize;
    public float accelerationFactor = .02f;

    public float minSize = 1e-5f;
    public float maxSize = 0.1f;

    void Start()
    {
        smoothSize = size;
    }

    public void HandleMovement()
    {
        size *= Mathf.Pow(0.5f, Input.GetAxis("Mouse ScrollWheel"));
        size = Mathf.Clamp(size, minSize, maxSize);
        smoothSize = Mathf.Lerp(smoothSize, size, Time.deltaTime);

        Vector3 move = Vector3.zero;
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = transform.right;
        right.y = 0;
        right = right.normalized;

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
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 1.5f); // smooth drag
        velocity = Vector3.ClampMagnitude(velocity, scaledMaxSpeed);

        position += velocity * Time.deltaTime;
    }
}
