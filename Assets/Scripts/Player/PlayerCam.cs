using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float sensX = 300f;
    public float sensY = 300f;
    public float sensitivityIncrement = 10f;
    public float smoothSpeed = 0.05f; // Lower value for more responsiveness

    [Header("References")]
    public Transform orientation;

    private float xRotation;
    private float yRotation;
    private bool isCursorLocked = true;

    private Vector2 currentMouseDelta;
    private Vector2 mouseDeltaVelocity;

    private void Start()
    {
        SetCursorLock(true);
    }

    private void LateUpdate()
    {
        HandleCursorLock();
        if (PlayerMovement.restrictMovementTokens > 0 || PlayerMovement.isCodexOpen) return;
        if (PlayerMovement.accessingInventory) return;
        HandleMouseInput();
        AdjustSensitivity();
    }

    private void HandleCursorLock()
    {
        bool shouldUnlock = PlayerMovement.accessingInventory || PlayerMovement.isCodexOpen;
        if (shouldUnlock && isCursorLocked)
        {
            SetCursorLock(false);
        }
        else if (!shouldUnlock && !isCursorLocked)
        {
            SetCursorLock(true);
        }
    }

    private void SetCursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
        isCursorLocked = lockCursor;
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        // Blend between raw and smoothed input for a more responsive feel
        currentMouseDelta = Vector2.Lerp(currentMouseDelta, new Vector2(mouseX, mouseY), smoothSpeed);

        yRotation += currentMouseDelta.x * Time.unscaledDeltaTime;
        xRotation -= currentMouseDelta.y * Time.unscaledDeltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void AdjustSensitivity()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            sensX += sensitivityIncrement;
            sensY += sensitivityIncrement;
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            sensX = Mathf.Max(sensX - sensitivityIncrement, 0);
            sensY = Mathf.Max(sensY - sensitivityIncrement, 0);
        }
    }
}
