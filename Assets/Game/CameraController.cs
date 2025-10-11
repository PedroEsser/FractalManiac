using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    public Camera cam;
    public float mouseSensitivity = 1.0f;
    public float yaw, smoothYaw = 0.0f;
    public float pitch, smoothPitch = 0.0f;

    public Vector3 smoothPosition = Vector3.zero;
    public float distanceToPlayer = 50;
    public ComputeShader computeShader;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        Cursor.visible = false;
        smoothPosition = GetPositionBehindPlayer();
    }

    void Update()
    {
        HandleMouseLook();
        computeShader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        computeShader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        computeShader.SetVector("_CameraWorldPos", cam.transform.position);
        computeShader.SetVector("_PlayerPos", player.position);
        computeShader.SetFloat("_PlayerSize", player.size);
        computeShader.SetVector("_ScreenParams", new Vector4(Screen.width, Screen.height, 0, 0));
    }

    void HandleMouseLook()
    {
        distanceToPlayer *= Mathf.Pow(0.75f, Input.GetAxis("Mouse ScrollWheel"));

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // prevent flipping

        float smoothTime = Mathf.Min(20f * Time.deltaTime, 1);
        smoothYaw = Mathf.Lerp(smoothYaw, yaw, smoothTime);
        smoothPitch = Mathf.Lerp(smoothPitch, pitch, smoothTime);

        transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);

        smoothPosition = Vector3.Lerp(smoothPosition, GetPositionBehindPlayer(), smoothTime);
        transform.position = smoothPosition;
    }

    private Vector3 GetPositionBehindPlayer()
    {
        return player.position - transform.forward * distanceToPlayer * player.size;
    }

    
}
