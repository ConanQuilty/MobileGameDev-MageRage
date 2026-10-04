*16/9/26*
I'm doing option 1: Arena Survivor Rougelite.
It's 'core verb' is spell casting. I would cut adding extra spells if time runs thin.

*20/9/26*
CPU main thread ms in a typical frame: 16.58ms
SetPass calls: 3
GC allocated in frame: 99B

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