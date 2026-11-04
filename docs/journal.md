*16/9/26*
I'm doing option 1: Arena Survivor Rougelite.
It's 'core verb' is spell casting. I would cut adding extra spells if time runs thin.

---
*20/9/26*
CPU main thread ms in a typical frame: 16.58ms
SetPass calls: 3
GC allocated in frame: 99B

---
*22/9/26*
| Test | Expected | Result |
|------|----------|--------|
| Press Home, wait 10 s, return | Paused, panel visible, audio silent, progress saved | Yes |
| Pull the notification shade down and up | Paused | Yes |
| Neighbour calls you, you hang up | Paused, game resumes only on Resume | Yes |
| Screen off with the power button, back on | Paused | Yes |
| Force stop from Settings, relaunch | Progress restored from the save | NA|

## Profiler capture, 2026-10-04 (Pixel 6a, 60 fps target)

- **Worst frame:** 576
- **Main-thread time:** 19.38 ms (VSync-capped, about 16.7 ms)
- **Tallest marker:** `PostLateUpdate.FinishFrameRendering`, 3.27 ms
- **Top three by self time** (waits and Profiler overhead excluded):
  1. `PostLateUpdate.FinishFrameRendering`, 3.27 ms
  2. 
  3. ___
- **GC.Collect:** yes / no (if yes: frame ___, ___ ms)
- **Notes:** most of the frame is idle (`WaitForTargetFPS`, about 14 ms), so real CPU work is only around 3 to 4 ms. GC allocation per frame is tiny (99 B).

---
*2/11/26*

Logging for future:
2026/11/02 15:55:02.251 26318 26353 Info Unity [Boot] Google Pixel 6a | Android OS 16 / API-36 (CP1A.260405.005/15001963) | OpenGLES3 | 2400x1080 @ 420 dpi | RAM 5589 MB | target 60 fps

*03/11/26* – W3 capture (Android Dev, Pixel 6a, 200-enemy crowd)
- Worst frame # 8517: main thread 17.01 ms, tallest marker: PostLateUpdate.FinishFrameRendering = 11.20ms largest child : Gfx.WaitForPresentOnGfxThread = 7.25ms
- Top 3 self ms: 
  1. PostLateUpdate.FinishFrameRendering (0.15 ms) 
  2. UniversalRenderPipeline.RenderSingleCam: MainCamera (0.17 ms) 
  3. Inl_on Record Render Graph (0.49 ms)
- Physics2D.Simulate 2.85 Time ms, calls 1 · Gfx.WaitForPresentOnGfxThread 7.25 Time ms
- GC.Collect: none
- SetPass 11 · Batches 1 · Triangles 2.2k
- Total Commited 444.1 MB · GC alloc in frame 4 99B
- [Baseline] (Cpu / Gpu bottlenecking?)
  - full scale: avg 16.67 ms  p99 16.71 ms  cpu 16.67 ms  gpu 12.84 ms 
  - half scale: avg 16.67 ms  p99 16.74 ms  cpu 16.67 ms  gpu 12.75 ms
  - Likely Cpu bound no real gpu improvement after swapping to half scale

Added debug menu for debug builds, allows stress testing through enemy spawning and changing render scale

---
*4/11/26* 
- Created 3 enemy types Grunt, Runner & tank. All 3 use same sprite so colors are changed to tell apart for now.
- Debug uses the Grunt enemy pool > Pushes it past the pool too

- Coroutine hunt 0 hits; WaveTimer written with Awaitable from the start (template: lab WaveTimer).
