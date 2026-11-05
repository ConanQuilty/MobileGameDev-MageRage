using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    public float moveSpeed = 8f;

    Camera cam; // only used to convert touch position to world position
    Rigidbody2D body;

    void Awake() { cam = Camera.main; body = GetComponent<Rigidbody2D>(); }
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void FixedUpdate()
    {
        if (LifecycleGuard.IsPaused || Touch.activeTouches.Count == 0) { 
            body.linearVelocity = Vector2.zero; 
            return; 
        }

        Touch t = Touch.activeTouches[0];

        float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector2 target = (Vector2)cam.ScreenToWorldPoint(new Vector3(t.screenPosition.x, t.screenPosition.y, depth));

        float speed = moveSpeed * stats.SpeedMult; 

        Vector2 next = Vector2.MoveTowards(body.position, target, speed * Time.fixedDeltaTime);
        body.linearVelocity = (next - body.position) / Time.fixedDeltaTime;
    }

}