using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Attach to your character. Moves the character toward the finger while touching.
// Does not move or clamp to the camera, so a camera that follows the player is fine.
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8f;                          // world units per second
    public bool snapToFinger = false;                     // true = teleport to finger
    public Vector2 fingerOffset = new Vector2(0f, 1.5f);  // keeps character visible above the finger

    Camera cam; // only used to convert touch position to world position

    void Awake() => cam = Camera.main;
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        if (LifecycleGuard.IsPaused) return;
        if (Touch.activeTouches.Count == 0) return;

        Touch t = Touch.activeTouches[0];

        float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 target = cam.ScreenToWorldPoint(new Vector3(t.screenPosition.x, t.screenPosition.y, depth));
        target.z = transform.position.z;
        target += (Vector3)fingerOffset;

        transform.position = snapToFinger
            ? target
            : Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}