using System;
using UnityEngine;

// Logs average and 99th percentile frame time every 600 frames (10 s at 60 fps),
// plus average CPU and GPU frame time from FrameTimingManager.
// All buffers are allocated once; the only garbage is the one log string per 10 s.
public class FrameTimeSampler : MonoBehaviour
{
    const int N = 600;
    readonly float[] _ms = new float[N];
    readonly float[] _sorted = new float[N];
    readonly FrameTiming[] _timing = new FrameTiming[1]; // reused every frame
    int _i;
    double _cpuSum, _gpuSum;
    int _timed;

    void Update()
    {
        _ms[_i++] = Time.unscaledDeltaTime * 1000f;  // unscaled: still measures while paused

        // Needs Player Settings > Frame Timing Stats. GPU time reads 0 on
        // devices/drivers that don't support it; then rely on RenderScaleProbe.
        FrameTimingManager.CaptureFrameTimings();
        if (FrameTimingManager.GetLatestTimings(1, _timing) > 0)
        {
            _cpuSum += _timing[0].cpuFrameTime;   // ms
            _gpuSum += _timing[0].gpuFrameTime;   // ms
            _timed++;
        }

        if (_i < N) return;
        _i = 0;
        Array.Copy(_ms, _sorted, N);
        Array.Sort(_sorted);                       // reused buffer, no garbage
        float sum = 0f;
        for (int k = 0; k < N; k++) sum += _sorted[k];
        float p99 = _sorted[Mathf.FloorToInt(0.99f * (N - 1))];
        double cpu = _timed > 0 ? _cpuSum / _timed : 0;
        double gpu = _timed > 0 ? _gpuSum / _timed : 0;
        Debug.Log($"[Baseline] avg {sum / N:F2} ms  p99 {p99:F2} ms  cpu {cpu:F2} ms  gpu {gpu:F2} ms");
        _cpuSum = _gpuSum = 0; _timed = 0;
    }
}