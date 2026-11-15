# Reflective Development Journal – Mage Rage (CA2)
**Starting thoughts:**
Initially when continuing the project I was very behind on labs.
My first thoughts were to do labs > game but that proved awkward as I soon realised i was being asked to stress test a very barebones game. I used a very basic red circle as an enemy for stress testing which provided minor results.

Doing things like creating multiple scenes for boot and result were a bit boring when I hadn't been working on the game that much

**Enemies:**
After that I started on creating enemies. I went for three basic types: Standard, Fast but weak and slow but tanky. I started by creating a basic enemy script that would take in stats to say what kind of enemy it was. Since I was behind I implimented the enemy pools at this stage too. In retrospect I should've just made the sprites for the enemies at this stage but I left it untill a later date.

**Magic Missile:**
I went on to create an auto firing projectile (Magic missile) as the first basic attack you have. I thought of doing a missile the direction you were holding but that would cause a player to run towards an enemy when shooting. These of course came from a pool.

**Enemy Re-use:**
I needed a way of tracking an enemies instance/ lifecycle because the missiles would get confused if they locked on to an enemy that died and got deactivated so i added life counters to enemies.
At the same time i was able to add kills to the UI and result screen.

**Flame Orb & Frost Nova:**
After adding 2 orbs circling arount the player that would deal damage called flame orbs. I didn't add any physics or effects to theese so they don't feel very impactful. This is something I'll need to change. I'm happy with frost nova though. I like it's slowing effect.

**Stats:**
I added player stats so that players could upgrade a stat after levelling up. This is something i'm definately going to have to revisit and tweak in the future. Some stats like attack now feel useless compared to attack speed. To get attack to work with small enemy health i changed both attack and enemy health to floats so that attack could raise from 1 > 1.1. Now that i'm taking the time to think back on these stats. The stat buffs on fire orbs and frost nova feel negligable so i'll need to revisit those.

**XP:**
Xp drops were added which came from a pool. I don't think anything about this needs to be changed. It's auto-pickup radius feels a bit small but it encourages the player to get closer to the enemy. I haven't run into issues with the xp pool as i had xp dissapear after time to further encourage players to gather it.

**Economy:**
Adding the economy loop was simple. Though the only thing you can do with it is increase attack damage. I'd like to make it so you buy your different big spells using coins to make it feel more impactful

**Cuts:**
On realising I hadn't left myself with enough time I've unfortunately cut the big spells for this deadline. Instead I will be adding those later, or at least one.

**Bottleneck fix:**
Trying to fix the bottleneck spotted earlier with physics collisions on enemy stress testing I removed enemy on enemy collision. This stopped the calculations between enemies but when spawning large quantities they will overlap and get hit with things like frost nova all together. This won't happen in a normal run so this change is kept.

**Ca3 next steps:**

Briefly the points I mentioned before:
- Big spells
- tweak level interaction with spells
- Economy for big spells