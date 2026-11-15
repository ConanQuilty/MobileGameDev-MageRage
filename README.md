# Mage Rage

A 3-minute top-down arena survivor for Android. Hold anywhere to walk the mage towards your finger; spells cast themselves. Survive four waves for 3:00, level up, and spend the coins you earn on Arcane Power between runs.


## Controls
| Action | How |
|---|---|
| Move | Touch and hold anywhere, the mage walks towards your finger |
| Spells | Automatic: Magic Missile from the start, Fire Orbs at level 3, Frost Nova at level 5 |
| Level-up | Tap one of five stats when the panel appears: HP +2, Defence +0.2 s invulnerability, Attack +10% damage, Speed +15%, Attack Speed +10% |
| Coins | Kills earn coins (Grunt 1, Runner 1, Tank 5); banked at the end of the run |
| Upgrade | Result screen: **Arcane Power** (+15% spell damage per level, cost 100 × 1.5^level) |
| Pause | Android Back (gesture or button), or leaving the app (Home, call, notification shade) |
| Back in menus | Goes up one level (Result > Menu) |
| Settings | Pause > Settings: haptics, screen shake, text size |

## Device targets
- **Tested:** Google Pixel 6a (6 GB RAM, Android 16).
- **Minimum API:** 26 (Android 8.0). **Target API:** Automatic (highest installed).
- **Orientation:** landscape.

## Build steps (clone to phone)
1. `git clone <repo URL>`.
2. Open with **Unity 6.6** with Android Build Support (SDK, NDK, OpenJDK).
3. **File > Build Profiles > Android**, Development Build **off** for release (on + Autoconnect Profiler for profiling only).
4. **Project Settings > Player > Android:** IL2CPP, ARM64 only, package `com.conanquilty.arenasurvivor`.
5. **Publishing Settings:** select the release keystore (see Signing) and enter both passwords.
6. **Build** to an `.apk`.
7. Install: copy the APK to the phone and open it from Files, or `adb install -r <file>.apk`.

Always start from the **Boot** scene in the Editor: it holds the persistent `App` object (LifecycleGuard, SceneLoader, Wallet).

## Known Issues
- **Enemies overlap into clumps** (Low): side effect of turning off Enemy↔Enemy collisions to cut Physics2D cost.
- **Fire Orbs kill clumps in bursts** (Low): many kills and XP gems land in one frame. CA3: cap hits per orb pass.
- **Tested on one device model only** (Med): Google Pixel 6a.
- **Not in this slice:** big spells (Bomb / Lightning / Super Mage) and the coin store were cut for time.

## AI assistance
I used AI as a debugging assistant for CA2.
- **What I asked:** Help with finding errors. Suggest solutions which I would check.
- **How I checked it:** I tested every suggested change on my device. Applied basic reasoning to responses.
- LLM used was Anthropic's Claude
