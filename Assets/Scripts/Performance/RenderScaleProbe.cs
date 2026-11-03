using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Halve the render scale. Frame time drops a lot: GPU-bound.
// Frame time barely moves: CPU-bound. Now you know where to look.
public class RenderScaleProbe : MonoBehaviour
{
    [Range(0.3f, 1f)] public float lowScale = 0.5f;
    UniversalRenderPipelineAsset _urp;
    float _full;

    void Start()
    {
        _urp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        _full = _urp.renderScale;
    }

    public void Toggle()                  // bind to the debug "Probe" button
    {
        bool atFull = Mathf.Approximately(_urp.renderScale, _full);
        _urp.renderScale = atFull ? lowScale : _full;
        Debug.Log($"[Probe] renderScale = {_urp.renderScale}");
    }

    // Added: in the Editor, changing the asset is saved to disk. Put it back
    // so you never commit a URP asset stuck at 0.5 (W7A Part C step 6 warns about this).
    void OnDestroy() { if (_urp != null) _urp.renderScale = _full; }
}