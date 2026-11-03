# Bottleneck 01 – 03/11/2026, Android Dev, Pixel 6a

**What:** With 200 enemies crowding the mage the game still holds the 60 fps cap (avg 16.67 ms, p99 16.71 ms, inside the 25 ms budget), but the GPU uses 12.8 of the 16.7 ms budget and Physics2D is the largest real CPU cost.
**Where:** GPU: `Gfx.WaitForPresentOnGfxThread` 7.25 ms (main thread waiting for the GPU inside `PostLateUpdate.FinishFrameRendering`, 11.20 ms total). CPU: `Physics2D.Simulate` 2.85 ms, 1 call per frame (physics keeping up). Largest self-time markers are all under 0.5 ms (`Inl_on Record Render Graph` 0.49 ms).
**Numbers:** worst frame #8517 main thread 17.01 ms (incl. waits) / steady __ ms · SetPass 11 · batches 1 · triangles 2.2k · GC alloc/frame 99B · no GC.Collect · Total Committed 444.1 MB ·
frame time full scale 16.67 ms (p99 16.71) vs half scale 16.67 ms (p99 16.74) · cpu 16.67 ms (includes vsync wait) / gpu 12.84 → 12.75 ms (FrameTimingManager)
**Verdict:** Not bound at 200 enemies: frame time sits on the 60 fps cap, so halving render scale shows 0% change. The GPU is the nearest limit (77% of the frame budget) and did not drop at half scale (−0.7%), so its cost is not pixel fill-rate. CPU real work is ~3–4 ms, led by Physics2D.
**Fix to try:** Cut Physics2D cost: turn off Enemy↔Enemy in the 2D Layer Collision Matrix and make enemies Kinematic, moved with MovePosition; measure `Physics2D.Simulate` ms and gpu ms before/after on the same route.

Evidence: `w03-profile.data`, `w03-bad-frame.png`, `[Baseline]` lines in docs/journal.md (03/11/2026).