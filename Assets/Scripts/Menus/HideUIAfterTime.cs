using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class HideeUIAfterTime : MonoBehaviour
{
    [SerializeField] float showForSeconds = 6f;
    float _hideAt;

    void OnEnable()  { EnhancedTouchSupport.Enable(); _hideAt = Time.time + showForSeconds; }
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        if (Touch.activeTouches.Count > 0 || Time.time >= _hideAt) gameObject.SetActive(false);
    }
}