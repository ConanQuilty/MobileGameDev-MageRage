using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TapSwipeInput : MonoBehaviour
{
    public float swipeDp = 50f;     // distance in dp, not pixels
    public float tapMax = 0.3f;     // seconds
    public float swipeMaxTime = 0.25f;

    public static event System.Action<Vector2> Tapped;  // screen position
    public static event System.Action<Vector2> Swiped;  // normalised direction


    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        if (LifecycleGuard.IsPaused) return; 

        foreach (var t in Touch.activeTouches)
        {
            if (t.phase != TouchPhase.Ended) continue;
            float px = swipeDp * Mathf.Max(Screen.dpi, 160f) / 160f;

            Vector2 d = t.screenPosition - t.startScreenPosition;
            double held = t.time - t.startTime;

            if (d.magnitude >= px) {
                if (held <= swipeMaxTime) { Swiped?.Invoke(d.normalized); }
                }
            else if (held < tapMax)
            {
                Tapped?.Invoke(t.screenPosition);
            }
        }
    }
}
