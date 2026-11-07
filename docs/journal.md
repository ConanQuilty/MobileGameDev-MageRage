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
- WaveTimer was written with Awaitable.WaitForSecondsAsync and a CancellationTokenSource linked to Application.exitCancellationToken.Bootstrap loads 
- Menu with await SceneManager.LoadSceneAsync.
- Stopping Play mode mid-wave gives no further [Wave] logs or spawns.

GC while spawning (Pixel 6a, Dev build, Autoconnected Player, w05-spawner.data):
- I added a ProfilerMarker ("EnemyPool.Spawn") so spawn frames can be found by name.
- Spawn frame 4898 (early in the run): EnemyPool.Spawn 0 B, no Instantiate. PlayerLoop 15.86 ms (under the 16.67 ms budget).
- The PlayerLoop total on that frame is 147 B, so the spawner is 0 B but something else in the frame still allocates. I haven't identified it yet; the prev capture showed a similar small amount (99 B)
- Spawn frame 5573 (later, wave 3): EnemyPool.Spawn 176 B. Nothing kills enemies yet, so the Grunt pool (120) empties after about 95 s and Spawn falls back to Instantiate. Makes sense as killing enemies not implemented yet.

Reflection: Pool sizes need better consideration down the line. Could be removing enemies as fast as they're created at one point but likely resoulution is to increase pool size.

Problems encountered:
- Creating a tmp label for time made 1m 4s show as 1:4 instead of desired 1:04. Fixed by using floats with {0:0}
- Overlaying huds conflicted with eachother (dev options hud blocked by player hud.) Fixed by unticking Raycast Target on display-only UI.
- Builds failed due to low space, opened up space by clearing Library/Bee

---

*05/11/26*
Added an auto-firing missile that homes on the nearest enemy in range every 0.5 s (base damage 2), from a prewarmed MissilePool.

- Enemies now keep a shared static list of who’s alive (Enemy.Active), so spells find targets without Find or physics queries. Nearest-enemy search compares squared distances to skip the square root.
- A Life (not hp) counter on each enemy stops a missile chasing a recycled enemy after its real target died.
- Kills are counted through a Killed event and shown on the HUD and Result screen.

- For Pool sizing a peak log shows what the size of all pools are post run.

- XP and level-ups. Kills drop pooled XP gems that fly to the mage; each level pauses for a choice.

- Levelling up gives the choices: HP +2, Defence +0.2 s invunerable time, Attack +10%, Speed +15%, and Attack speed +10%, which I split into its own stat.
- Freezing time: I extended LifecycleGuard with a “hold” counter, so the level-up freeze survives Home > Resume. Before, Resume would have restarted time behind the open panel.
- Damage as floats: I spotted that % boosts don’t fit whole-number damage. Rounding meant a second Attack pick did nothing, so enemy health and spell damage are now floats.

- Added Fire Orbs and Frost Nova. Orbs circle the mage from level 3, and Nova pulses a slow every 4 s from level 5. Both use distance checks, not physics, to keep the Physics2D cost unchanged for the bottleneck test. The orbs looked too small, so they now size themselves from their hit radius, and the visual always matches the damage area.

---

*07/11/26* 
_breaking the loader on purpose_

- For the hang test I changed the SceneLoader loop from while (op.progress < 0.9f) to while (!op.isDone). The bar filled and stayed full, and the Game scene never appeared (screenshot: docs/CA2/baseline/loader-hang.png). 
- When allowSceneActivation = false, Unity stops progress at 0.9 and waits for permission to switch scenes, so isDone never becomes true and the loop never exits, which gives an infinite loading screen. Reverting to progress < 0.9f, then setting allowSceneActivation = true and awaiting the operation fixes it.

- When hitting home mid-load. Tapped Play, pressed Home straight away, waited 5 s, then returned. The load [finished / the loading screen disappeared and Game started]. It survives because the token I pass is Application.exitCancellationToken which only cancels on quit, and nothing in OnApplicationPause cancels it.

- Added if (SceneLoader.IsLoading) return; to PauseMenu and ResultScreen. When pressing Back during the bar did nothing: no pause panel and no jump to Menu.

- What I learned: a loop that waits for “done” can wait forever if something else is holding the operation back. Check what actually changes (progress reaching 0.9), not what you assume will (isDone).


