using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Attach to your character. Moves the character toward the finger while touching.
// Does not move or clamp to the camera, so a camera that follows the player is fine.
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8f;                          // world units per second
    public Vector3 fingerOffset = new Vector2(0f, 1.5f);  // keeps character visible above the finger

    Camera cam; // only used to convert touch position to world position
    Rigidbody2D body;

    void Awake() { cam = Camera.main; body = GetComponent<Rigidbody2D>(); }
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void FixedUpdate()
    {
        if (LifecycleGuard.IsPaused || Touch.activeTouches.Count == 0)
        {
            body.linearVelocity = Vector2.zero;   // when no touches reset velocity to fix drift after enemy collision
            return;
        }

        Touch t = Touch.activeTouches[0];

        float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 target = cam.ScreenToWorldPoint(new Vector3(t.screenPosition.x, t.screenPosition.y, depth)) + fingerOffset;

        Vector2 next = Vector2.MoveTowards(body.position, target, moveSpeed * Time.fixedDeltaTime);
        
        body.linearVelocity = (next - body.position) / Time.fixedDeltaTime;
    }
}