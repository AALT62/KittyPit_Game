using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Transform cameraPivot;

    [Header("Settings")]
    public float rotationSpeed = 3f;
    public float pivotSpeed = 2f;
    public float minPivotAngle = -30f;
    public float maxPivotAngle = 60f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -4f);

    private float lookAngle;
    private float pivotAngle;
    private Transform mainCameraTransform;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        HandleRotation();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        lookAngle += Input.GetAxis("Mouse X") * rotationSpeed;
        pivotAngle -= Input.GetAxis("Mouse Y") * pivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        playerTransform.eulerAngles = rotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        cameraPivot.localEulerAngles = rotation;
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition = playerTransform.position +
                                playerTransform.forward * cameraOffset.z +
                                playerTransform.up * cameraOffset.y +
                                playerTransform.right * cameraOffset.x;

        mainCameraTransform.position = targetPosition;
        mainCameraTransform.LookAt(playerTransform.position + Vector3.up * 1.5f);
    }
}