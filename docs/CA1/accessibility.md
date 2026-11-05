# Accessibility pass – Mage Rage 0.2.1 (Pixel 6a, Android 16) – 30 Oct 2026

| Check | Finding / Fix |
|---|---|
| One-handed reach: every control reachable with the thumb (landscape, either hand) | No |
| Smallest button ≥ 48 dp incl. padding (130 px at 1920×1080 ref, Match = 1) | |
| Settings > Display & touch > Display size and text at max: HUD still inside safe area | Yes |
| Developer options > Simulate colour space > Monochromacy: every state readable | |
| Volume 0: game still shows what happened (hit flash, HP bar) | |
| Text contrast ≥ 4.5:1 (colours: text #____ on #____, tool: webaim.org/resources/contrastchecker) | |
| Motion / screen shake behind a toggle | Done: Settings > Screen shake (stored in PlayerPrefs `screenShake`) |
| No timed tap without an alternative | Movement is hold-to-move, big spell has no timing window: none |
| Haptics off: nothing lost (every buzz has a visual too) | |
| 60 s silent observation: first thing the tester got wrong | |

**Fixes done today:** …
**On the cuts list / later:** …