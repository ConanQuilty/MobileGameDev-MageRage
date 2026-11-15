# Economy & Telemetry Map – Mage Rage

| Event | Parameters | Fires when | Question it answers |
|---|---|---|---|
| `session_start` | `app_version` (string), `device_model` (string), `android` (string) | Boot `Awake` | Which build and device made this log |
| `level_start` | `level_id` (int), `attempt` (int), `coins` (int) | Game scene starts | Retries per session; how rich players are going in |
| `level_complete` | `level_id` (int), `time_s` (int), `coins_earned` (int), `score` (int, kills) | Survived 3:00 | Earn rate on a win |
| `level_fail` | `level_id` (int), `time_s` (int), `cause` (id), `coins_earned` (int) | HP hits 0 | When and why players die; earn rate on a loss |
| `upgrade_purchased` | `upgrade_id` (id), `level` (int), `cost` (int), `coins_left` (int) | `Wallet.TryUpgrade` succeeds | How many runs until the first upgrade; spend pacing |

**Where the data lives:** events are written to logcat (`[telemetry]` tag) and appended to `telemetry.log` in `Application.persistentDataPath`. The data never leaves the device: there is no SDK and no network use. The privacy statement (`docs/CA1/privacy-statement.md`) says the same.

**Economy loop:** kills earn coins (Grunt 1, Runner 1, Tank 5), the run's coins are banked into the Wallet once at run end, and they are spent on the Result screen on Arcane Power (+15% spell damage per level, cost 100 × 1.5^level: 100, 150, 225, 337…).
