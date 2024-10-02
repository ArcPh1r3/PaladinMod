## Changelog
`2.0.3`
- fixed russian translation errors

`2.0.2`
- fixed collision on corruption skin
- fixed missing mastery unlock string
- added Spanish (ES) (thanks Bagre)
- added Spanish (LatAm) (thanks Juhnter)
- added Russian (thanks Lecarde)
- re-enabled French, Chinese, Italian, and Portuguese with correct numbers, but achievement translations are still incomplete.
- removed FixInivincibleMithrix dependency

`2.0.1`
 - fixed emotes to new rig. disable paladin support config option in CustomEmotesApi config

`2.0.0`
 - Updated for SotS. 
     - *Generally working, but a lot of issues are still present. Some effects are missing, some error spams in the log* 
     - *please report anything you can find, with logs, on discord or github issues*
 - Big ol animation update courtesy of domi and by courtesy I mean commissionned by jame and by of domi I mean several months ago sorry it took so long and it's not even done
     - absurdly high quality animations for everything
     - second set of animations for out of combat
         - second-and-a-half set of animations for dragging while sprinting
     - *with this level of animation there are bound to be spots where transitions are off. please report on discord or github issues*
     - added a new "LimitBreak" animation under "FullBody, Override" and "Gesture, Override" for use by other mods
 - All skins were broken and had to be redone to accommodate a new rig for the animations
    - if you made a skin it is now broken, You'll have to skin it to the new rig, found [here](https://github.com/ArcPh1r3/PaladinMod/blob/master/Blend/Paladin_rig_for_skins.blend). reach out if you have any questions
 - Gameplay changes courtesy of rob
    - Base max health: 160 > 148
    - Health per level: 64 > 44.4
    - Base health regen: 1.5 hp/s > 1 hp/s
    - Health regen per level: 0.3 hp/s > 0.2 hp/s
    - Base damage: 13 > 12
    - Damage per level: 2.6 > 2.4
    - Base armor: 10 > 20
    - Armor per level: REMOVED
    - Passive: NEW EFFECT - While active, grants +1 armor per level and 3 + (0.6 * level) hp/s to all allies (including self) in a 48m radius, and speeds up spellcasts by 20%
    - Divine Blade damage: 350% > 300% (both beam and slash)
    - Spinning Slash damage: 1000% > 700%
    - Spinning Slash NEW EFFECT: Now reduces Quickstep cooldown on hit (once per use)
    - Lightning Spear damage: 800% > 700%
    - Quickstep barrier amount: 10% > 15%
     - *the aim is to shift power away from raw stats and damage and give him more durability when played well, as well as a team bolstering passive to make him a proper team player*
 - added language support
    - *currently disabled until they are updated to the new strings. if you would like to help, I'd be eternally grateful*
    - added French courtesy of StyleMyk
    - added Brazilian Portuguese courtesy of Kauzok
    - added Chinese courtesy of MushroomEl
    - added Italian coutesy of oiakam
- misc
    - split assetbundle and all that good stuff
    - finally addressed the ridiculous polycounts of the models
    - adjusted paladin UVs and textures that absolutely no one will notice lol
    - flipped symbol on GM cape it was upside down lol
    - legacy nkuhana and lunar knight skins capes rigged to the new cape bones

*VR was not tested at all, sorry. please report any issues to the github issues page. thank you*

`1.6.3`
 - fixed eclipse not saving
 - cruel sun now scales with attack speed

`1.6.2`
 - fixed vr code messing with enforcer vr
 - fixed utility using all its charges on one cast
 - fixed scepter making legacy cruel sun last indefinitely
 - brought pride flare explosion damage down to 4000 (from haha funny 9001)
 - changed pride flare explosion falloff to Linear (from Sweetspot)
 - changed config to add legacy cruel sun as an alternate rather than replace the existing
 - added a handful of dlc1 item displays
   - the rest are there commented out if someone wants to do them for us

`1.6.1`
 - new cruel sun
   - lowered initial cast time 
   - increased damage 150% -> 160% because why not
   - increased range 60 -> 70
   
*some felt the move wasn't strong enough to warrant the new commitment to self damage.*   
*hopefully these changes make it feel a bit better to use*

 - fixed pride flare scepter text
 - still didn't fix pride flare 9001% damage. have fun
 - added missing weakpoint hitbox

`1.6.0`
 - Full VR Implementation. see VR section above
   - *Thanks PureDark!*
 - Cruel Sun Rework
   - *Implemented actual months ago by Varna. Thanks and sorry it held for so long*
   - No longer placed at an aim point. Now is held consistently above you
   - Ally damage and burn stacks reduced heavily, allowing for controlled use against enemies
   - can be canceled by pressing R, Utilities, or Sprinting
   - Scepter: Pride Flare: be aimed and thrown to detonate for 9001% to all characters
 - inferno compat for grand mastery skin

`1.5.10`
 - moved unlockable code to r2api, fixing achievement issue with recent update 

`1.5.9`
- fixed m1 not working with shuriken properly
- improvements to m2: made hitbox last the whole movement, extended visuals to better communicate hitbox size
- fixed skill selections not being remembered. excuse the "new" highlights

`1.5.8`
- grandmastery skin now achievable in eclipse
- fixed drip skin sounds being too quiet
- fixed achievements hidden in logbook
- fixed missing buff icons

`1.5.7`
- returned item displays, may need fixing, not including SotV content
- potential fix for void dios bug

`1.5.6`
- fix sounds being too quiet (thanks Michaell for testing)
- let me know if any are too loud now or still too quiet
- man fuck sounds

`1.5.5`
- fixed for GUPdate
- adjusted camera logic. let me know if something looks off on his skills and emotes
- fixed giant burning and poison projectiles
- had to redo sounds. let me know if some sounds are too loud/quiet/missing

`1.5.4`
- Deprecated Aetherium item displays, but in a good way. Komrade's added the displays on his end. Say thank you!
- Grandmastery skills no longer require starstorm to be installed if they've been unlocked. 
  - (still needs starstorm's typhoon or higher difficulty complete the achievement)
- made mastery and grandmastery support any higher modded difficulty

`1.5.3`
- added safety net in item displays code in case theoretically an update to an item mod happens soon that would be conflicting you know just in case hypothetically

`1.5.2`
- fixed lunar knight skin css effect that I missed

`1.5.1`
- Added skin-specific sword beam colors
- Added some tools for custom skin mods to have their own sword beam colors and lobby effects (see Custom skin guide above)
- Added some skins back under a legacy skins config
- reaching old enforcer levels of too many skins (rip). dulled config skins' icons to distinguish normal skins
- Updated/fixed item displays
- re-added item displays for aetherium
- fuck you supply drop for having a plague mask obligating me to set up your displays
- added ragdoll to creepy arms on Corruption skin
- fixed material for floating crown things on sovereign skin
- deleted the 'remove item displays' config cause fuck the unworthy

`1.5.0`
- Back under a new team account :O.
- Thanks for everything rob ur dick is _this big_ and I said that's disgusting so I'm making a call out post on my twitter dot com
- Huge updates to Mastery (Lunar), GrandMastery (Sovereign), and Corruption skins. (thanks SkeletorChampion the models are sexy)
- Tweaks to Paladin's base model and aphelian skin
- New Specter skin in cursed config
- Tweaks to animations: Idle, walkF, menuIdle, menuIdlein
- Shrunk css display a bit to fit on screen lol (didn't change him in-game don't worry)
- Sword effects now change with skins in css
- Retimed activation on swing attacks and it feels so much better now
- Added new emote orignally for testing but fuck it its in now

`1.4.15`
- Updated spell animations and VFX
- Replenish now has a short channel time
- Cruel Sun cooldown 48s > 40s
- Pride Flare detonation radius 64m > 80m
- Pride Flare detonation knockback force 0 > 8000
- Channeling spells no longer activates Red Whip
- Sacred Oath bonus damage 50% > 25%
- Sacred Oath buff duration 1s > 2s
- Fixed buggy Sovereign unlock

`1.4.14`
- Fix Sunlight Spear sometimes granting the buff permanently
- Sunlight Spear buff range 12m > 16m

`1.4.13`
- Sunlight Spear cooldown 6s > 8s
- Sunlight Spear damage 1000% > 800%
- Sunlight Spear new effect: hitting yourself with the bolt now coats your blade in lightning for 4 seconds, causing your attacks to apply chain lightning for 75% TOTAL damage
- Overhauled Sunlight Spear visuals
- Updated all run animations
- Updated various misc VFX
- Added a handful of item displays

`1.4.12`
- Updated ALL skill icons! (thanks OK)
- Lunar Shards damage 225% > 75%
- Fixed Sceptered Cruel Sun not working in multiplayer

`1.4.11`
- Fixed Sceptered Cruel Sun doing way less damage than intended

`1.4.10`
- Fixed Sceptered Cruel Sun not working in multiplayer

`1.4.9`
- Restored Ancient Scepter functionality

`1.4.8`
- Fixed for latest RoR2 update
- Shields no longer count toward passive threshold
- Lunar Shards cooldown 0.75s > 1.5s once the last shot was fired
- Lunar Shards now recharge all stock rather than just one
- Fixed missing Sunlight Spear impact sound
- Fixed missing drip emote animation

`1.4.7`
- Readded drip emote
- Added unlock condition for Cruel Sun
- Rewrote camera logic, added new camera position for spellcasts
- Spinning Slash(grounded) subsequent spins are now faster
- Updated spell cast area indicators to add some more flavor
- Fixed Cruel Sun being buggy for clients
- Fixed Cruel Sun not being placed directly where aimed
- Fixed Spinning Slash sometimes locking you out of jumps

`1.4.6`
- Fixed Cruel Sun not working in multiplayer
- Fixed Cruel Sun's duration scaling with attack speed
- Replenish heal amount 10% > 15%
- Replenish barrier amount 10% > 15%

`1.4.5`
- Added a new experimental special, Cruel Sun
- Added unique Mithrix quotes for encountering and killing Paladin
- Added Vow of Silence skill icon
- Tweaked mastery skin's cape color

`1.4.4`
- Updated texture
- Reimplemented all skins
- Added particles to the alt run animation
- Added animations for the Heresy skills
- Added config option to add a cape because it's cool
- Added a handful of item displays
- Lunar Shards can now be fired alongside other skills
- Fixed jumping during Spinning Slash
- Fixed Paladin's body turning white on lower texture resolutions
- Fixed sword beams doing inconsistently high damage

`1.4.3`
- Reverted run animation, new animation now plays while out of combat instead
- Updated some animations
- Added mastery skin back
- Canceling a spell channel with sprint now refunds 90% of the cooldown
- Fixed awkward camera position while channeling spells
- Added logbook lore, written by James- due to a vanilla issue it does not save properly
- Added an eye trail because eye trails are just cool
- Small tweak to attack hitstop for more fluid movement

`1.4.2`
- Fixed an issue causing Paladin to be unplayable sometimes

`1.4.1`
- Nerfed size a bit and tweaked camera position
- Slightly increased sprint speed
- Fixed Tri-Tip Dagger breaking animations

`1.4.0`
- Fixed mod to work with RoR2's anniversary update
- Updated a lot of animations
- Spinning Slash(grounded) now smoothly cancels into itself
- Increased size
- Skins and item displays temporarily removed, will be added back in a later update

`1.3.3`
- Migrated to Standalone Ancient Scepter, removed support for ClassicItems

`1.3.2`
- Fixed the mysterious disappearing Paladini

`1.3.1`
- Fixed missing cape and giant SS2 item displays

`1.3.0`
- Increased Spinning Slash damage from 800% to 1000%
- Added Grand Mastery skin- model by SkeletorChampion
- Updated Aphelian skin- once again by Skeletor
- Updated Spinning Slash description to remove some clutter
- Updated some sword VFX
- Added item displays for missing Supply Drop items
- Fixed skin unique item displays not working
- Fixed Interstellar Desk Plant
- Fixed Shielding Core rotation
- Moved joke skins to cursed config- meant to do it a long time ago but forgot
- Emotes can no longer be used while chat is active
- Networked hit sounds

`1.2.6`
- Fixed item displays
- Slowed down grounded Spinning Strike animation, increased lunge distance

`1.2.5`
- Fixed modded item displays
- Tweaked Lunar and Clay skin
- Tweaked walk and jump animations
- Added new grounded Spinning Slash animation
- Increased grounded Spinning Slash slow, added a small forward lunge
- Lowered Sacred Oath cast time from 2.5s to 2s

`1.2.4`
- Fixed incompatibility with Dark Souls skins

`1.2.3`
- Sacred Oath now gives attack speed rather than armor
- Tweaked Minecraft skin rig and added new sword(1.2.1 change, forgot to list)
- Fixed The Backup item display
- Massive backend refactor to make things easier to understand/work with and simplify character creation as well as support multiple character additions- hopefully should serve as a solid example for learning character creation now

`1.2.2`
- Fixed Quickstep animation

`1.2.1`
- Updated Sunlight Spear animation
- Tweaked sprint animation
- Added Point Down emote(bound to 3 by default)
- Added lots of new VFX for Dragonyck's Dark Souls skins
- Made Paladin properly hold Tri-Tip and Shattering Justice when acquired
- Fixed some item displays

`1.2.0`
- Finished item displays, including support for certain mods- Aetherium, SupplyDrop and SivsItems
- Updated all spell effects
- Lowered the effectiveness of attack speed on aerial Spinning Slash to 25% to stop attack speed from hindering your mobility
- Fixed bugs with permanent Dark Souls shields
- Rewrote Oath of Silence's projectile destruction, fixing performance issues
- Rewrote skin VFX system to make it easier to maintain and support new skins as well as prevent bugs caused by disabling skins

`1.1.3`
- Fixed sword beam damage

`1.1.2`
- Added a new skin- big thanks to SalvadorBunny for his amazing work on it!

`1.1.1`
- Fixed the new skin stealing the drip emote
- Reverted Quickstep sprint behavior as it wasn't well received at all

`1.1.0`
- Added a new skin
- Added custom passive lightning for certain Dark Souls skins
- Added Quickstep effects
- Quickstep no longer forces a sprint(last fix didn't work)
- Heavily buffed Sacred Oath(radius, stats, duration, etc don't remember it all)

`1.0.9`
- Fixed Sacred Sunlight bug
- Fixed Scepter upgrades not actually casting the upgraded spells

`1.0.8`
- Added unlock challenges for alt skills
- Added a new special, Sacred Oath- a channeled spell that grants +30% damage and +50 armor for a short time
- Removed the armor buff from Sacred Sunlight
- Lowered Sacred Sunlight barrier per tick from 1.5% back to 1%
- Lowered Quickstep barrier amount from 15% to 10%
- Quickstep no longer forces sprint
- Renamed Heal to Replenish
- Lowered Replenish barrier amount from 20% to 10%
- Added Ancient Scepter upgrades for all specials, adjusted VFX
- Hallowed Sunlight: Heal amount +100%, radius +50%, barrier +50% 
- Oath of Silence: Slow amount 60% > 80%, now destroys projectiles
- Sacred Oath (Scepter): Damage buff +100%, armor buff +100%, radius +50%
- Tweaked cloth to be a little less rigid
- Removed all reflection from code, should improve performance
- Actually fixed jiggly items
- Did not add stat config

`1.0.7`
- Fixed kills from Vow of Silence not granting kill gold
- Spells now apply one tick of their effect instantly when casted
- Tweaked bat SFX/VFX
- Tweaked walk animation
- Added custom emote for Drip skin
- Added shield for Havel skin, fixed some mismatched VFX
- Added a couple more item displays

`1.0.6`
- Tweaked Lunar Shard aim some more
- Added cape to N'kuhana skin, tweaked various other things
- Adjusted spell VFX
- Added custom Aegis displays for Dragonyck's Dark Souls skins
- Added custom red VFX for Pursuer skin
- Added config option for disabling item displays

`1.0.5`
- Added a new skin
- Added some spell VFX
- Added spawn animation
- Tweaked animations some more, this time focusing mainly on the awkward cape movement
- Consecutive aerial Spinning Slashes now stack up damage if nothing is hit, consuming all stacked damage on first enemy hit
- Rewrote Quickstep for the 3rd(4th?) time- this should solve all issues with inconsistent movement
- Quickstep no longer counts as a combat skill

`1.0.4`
- Tweaked some animations, right arm should no longer be weird with custom skins
- Smoothed out primary transitions
- Lunar Shard aim adjusted
- Aiming spells no longer hides the crosshair
- Added some sounds
- Added a 1.5s channel time to Sacred Sunlight before it can be cast
- Sacred Sunlight barrier per tick increased from 1% to 1.5%
- Sacred Sunlight duration increased from 10s to 12s
- Sacred Sunlight cooldown lowered from 24s to 18s
- Added a 2s channel time to Vow of Silence before it can be cast
- Vow of Silence slow strength increased from 60% to 80%
- Vow of Silence duration increased from 8s to 10s
- Vow of Silence cooldown lowered from 24s to 18s
- Added a visual effect for Torpor
- Added a couple more item displays

`1.0.3`
- Added a handful of item displays
- Fixed passive not activating at all with shields

`1.0.2`
- Shields are now counted for passive activation
- Passive description now properly states the threshold of 90%(was changed previously but description was never updated to match)
- Base armor lowered from 15 to 10
- Base health regen lowered from 2.5/s to 1.5/s
- Primary damage lowered from 375% to 350%
- Sunlight Spear max damage lowered from 1200% to 1000%
- Lunar Shard damage increased from 200% to 225%
- Quickstep barrier lowered from 20% to 15%
- Heal radius increased from 12m to 16m
- Heal amount lowered from 20% to 10%
- Heal now grants 20% barrier
- Sacred Sunlight radius increased from 14m to 16m
- Sacred Sunlight armor buff lowered from 10 to 5
- Sacred Sunlight barrier per tick lowered from 1.5% to 1%
- Sacred Sunlight duration increased to 10 seconds
- Fixed skin VFX system breaking with certain skins

`1.0.1`
- Added custom VFX for skins- this includes support for Dragonyck's Dark Souls skins, check that out if you haven't already
- Tweaked some textures and skill icons
- Made sword glow in character select
- Fixed slight stutters whenever an enemy spawns
- Fixed rest emote not being networked properly
- Fixed global swing sounds

`1.0.0`
- Huge overhaul!!
- Updated model and animations, all remade from the ground up! (model by Zenta)
- Updated a lot of VFX
- Added custom sounds
- Added ragdoll
- All hitboxes greatly improved
- Lowered base damage from 14 to 13
- Lowered primary damage to 375%, removed passive bonus damage because the sword beam already increases damage if you hit both so it was redundant
- Increased primary sword beam damage to 300%
- Increased primary base duration
- Grounded Spinning Slash range greatly increased
- Aerial Spinning Slash can now cancel into itself with backup mags
- Aerial Spinning Slash passive bonus damage removed- now fires a shockwave upon landing for 300% damage
- Sunlight spear no longer pierces, now explodes on impact
- Spellcasts no longer have a weird lingering duration that prevents you from doing anything
- Made some changes to emotes- 1 by default is now a rest emote, Praise the Sun moved over to 2 default, Point Down not yet reimplemented, more planned
- Fixed a skin being unlocked seemingly at random- was actually whenever a Malachite elite spawned
- Removed Hunter skin for now- expect it back at some point
- Item displays out for now- will be added back over time, but had to be trashed due to the new model(good thing I never put in the hours to add all of them before this)
- A bunch of other changes that I probably missed

`0.0.8`
- Base damage/growth lowered to 14/2.8
- Fixed Phase Round Lightning applying on sword beams
- Buffed Lunar Shard damage to 200%, adjusted cooldown mechanics
- Lunar Shard animation cancel somewhat restored
- Fixed torpor not grounding enemies,, enjoy slamming vagrants into the ground
- Increased Vow of Silence radius
- Made spells castable on enemies

`0.0.7`
- Torpor change may not have been networked, fixed that

`0.0.6`
- Updated character portrait
- Updated skins
- Improved melee hitboxes
- Passive now activates with barrier as well as >80% health
- Increased base damage/growth from 12/2.4 to 15/3
- Lowered empowered primary damage from 600% to 500% to compensate for base damage buff
- Tweaked aerial Spinning Slash movement to help with flying enemies
- Fixed Lunar Shards animation cancelling and nerfed damage
- Increased Quickstep barrier amount from 15% to 20%, increased base dash distance
- Cleaned up Quickstep visuals
- Torpor now grounds enemies- slow harshly nerfed to compensate
- Vow of Silence visual is now red, still placeholder, but different enough to prevent confusion

`0.0.5`
- Tweaked sword color
- Fixed sword hitboxes
- Added a new secondary, Lunar Shards
- Tweaked aerial Spinning Slash some more
- Rewrote Quickstep, added an extra charge(animations in progress)

`0.0.4`
- Tweaked aerial Spinning Slash movement
- Removed air dashes, Quickstep now always functions the same

`0.0.3`
- All skins now have cloth
- Minor animation update
- Added a new skin
- Lowered armor per level to 1
- Big gameplay overhaul, aiming to make him overall more interactive and better support melee playstyles

Primary

- Divine Blade
-- Lowered base duration from 1.3s to 1s
-- Lowered base damage from 450% to 400%(buffed damage unchanged)
-- Buffed sword beam damage from 225% to 300%, increased range/speed/hitbox size, updated VFX, added keyword
-- Adjusted hitstop duration
-- Updated icon

Secondary

- Sunlight Spear
-- Damage changed from 100%-2400% to 200%-1200%
-- Charge time lowered from 2s to 1.5s
-- Moved to secondary slot, now agile
-- Cooldown lowered to 8s
-- Updated icon

- Spinning Slash
-- Dash removed, moved to its own skill in the utility slot
-- Damage increased to 800%
-- Base duration increased to 1.2s
-- Cooldown increased to 6s
-- Range now increased while passive is active
-- Aerial version performs Artorias' leap strike, damage increased by passive instead
-- Updated icon

Utility

- Quickstep (NEW)
-- Use while grounded to dash in a direction, use while airborne to dash forward
-- Grants i-frames during the dash and 15% barrier once it ends
-- Hitting stuff with Divine Blade lowers the cooldown(more atk spd = more barrier = more tank)

Special 

- Sacred Sunlight
-- Heal amount greatly reduced, armor buff greatly reduced, now grants some barrier

- Vow of Silence
-- Fixed performance issues, current visual is placeholder
-- No longer destroys projectiles(might come back eventually)

`0.0.2`
- Fix typo

`0.0.1`
- Initial release