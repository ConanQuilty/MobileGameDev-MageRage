using UnityEngine;

// Hides an object in release builds.
public class DebugOnly : MonoBehaviour
{
    void Awake() => gameObject.SetActive(Debug.isDebugBuild);
}