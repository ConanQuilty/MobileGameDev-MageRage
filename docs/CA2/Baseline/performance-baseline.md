# Performance Baseline 
**Build measured:** `MageRage`, IL2CPP ARM64, 15 Nov 2026, Google Pixel 6a. Target 60 fps (16.67 ms); budget p99 < 25 ms.

## Device
| Field | Value | Source |
|---|---|---|
| Model / OS | Google Pixel 6a / Android 16 (API 36) | `[Boot]` log line |
| SoC / GPU | Google Tensor (GS101) / Mali-G78 MP20 | Device specification |
| RAM | 6 GB | Device specification |
| Battery health estimate | <fill: Settings > Battery > Battery health, or "not shown"> | |
| Display / render | 2400×1080 @ 420 dpi, 60 Hz, render scale 1.0 | `[Boot]` log line |

## Frame time, memory, start-up and size
| Measure | Release (15 Nov) | Earlier |
|---|---|---|
| Frame time, normal run: avg / p99 | **16.27 / 16.49 ms** (500 Enemies) | W3 (3 Nov, 200 enemies): 16.67 / 16.71 ms |
| CPU vs GPU (FrameTimingManager) | cpu 16.67 ms (includes vsync wait) / **gpu 11.68 ms** | W3: cpu 16.67 / gpu 12.84 ms |
| Bound | Not bound: frame time holds the 60 fps cap; GPU uses ~70% of the budget | W3 render-scale test: full vs half scale 16.67 vs 16.67 ms, so not fill-rate bound |
| GC alloc per frame | n/a in release | ~147 B/frame steady; `EnemyPool.Spawn` 0 B when the pool is sized |
| Peak memory | 309.647mv | W3: 444.1 MB Total Committed  |
| Draw calls | n/a in release | W3, 200 enemies: SetPass 11 - batches 1 - triangles 2.2k  |
| Cold start to interactive (3 runs) | 2.77 s, 2.63 s, 3.09 s > **median 2.77 s** (all ≤ 5 s) | |
| Scene load Menu > Game (3 runs) | 163, 148, 207 ms > **median 163 ms** | 7 Nov: 125, 155, 132 ms |
| APK size | **34.5 MB** | Dev APK 7 Nov: 33 MB |

## Thermals
| Start temp | End temp / delta | Throttling  | Session length |
|---|---|---|---|
| 27.8 °C | 28 °C  | No throttling | 10 min |

## Bottleneck 01 – 03/11/2026

**What:** With 200 enemies crowding the mage the game still holds the 60 fps cap (avg 16.67 ms, p99 16.71 ms, inside the 25 ms budget), but the GPU uses 12.8 of the 16.7 ms budget and Physics2D is the largest real CPU cost.

**Where:** GPU: `Gfx.WaitForPresentOnGfxThread` 7.25 ms (main thread waiting for the GPU inside `PostLateUpdate.FinishFrameRendering`, 11.20 ms total). CPU: `Physics2D.Simulate` 2.85 ms, 1 call per frame (physics keeping up). Largest self-time markers are all under 0.5 ms (`Inl_on Record Render Graph` 0.49 ms).

**Numbers:** worst frame #8517 main thread 17.01 ms, batches 1, triangles 2.2k, GC alloc/frame 99B, no GC.Collect, Total Committed 444.1 MB 
frame time full scale 16.67 ms (p99 16.71) vs half scale 16.67 ms (p99 16.74) - cpu 16.67 ms (includes vsync wait) / gpu 12.84 > 12.75 ms (FrameTimingManager)

**Verdict:** Not bound at 200 enemies: frame time sits on the 60 fps cap, so halving render scale shows 0% change. The GPU is the nearest limit (77% of the frame budget) and did not drop at half scale (−0.7%), so its cost is not pixel fill-rate. CPU real work is ~3–4 ms, led by Physics2D.

**Fix to try:** Cut Physics2D cost: turn off Enemy↔Enemy in the 2D Layer Collision Matrix and make enemies Kinematic, moved with MovePosition; measure `Physics2D.Simulate` ms and gpu ms before/after on the same route.

**Evidence**: `w03-profile.data`, `w03-bad-frame.png`, `[Baseline]` lines in docs/journal.md (03/11/2026).
Later note: I've had to remove some profiler data from the git as it was too large and causing issue

## Revisit bottleneck 15/11/26

**What the bottleneck looks like now:** 500+ enemies causing [Baseline] avg 16.73 ms  p99 16.77 ms  cpu 16.73 ms  gpu 12.34 ms
Physics2d.Simulate: 6.08 ms

**Fix attempt: remove enemy collision.** 
What does that look like? [Baseline] avg 16.70 ms  p99 16.73 ms  cpu 16.70 ms  gpu 9.24 ms

**Side effects:** Enemies in large swarms will overlap.

**Evidence:** see PhysBeforeCollisionOff.data and PhysAfterCollisionRemove.data