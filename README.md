#Battle Slots
![alt tag](https://raw.githubusercontent.com/adamjoyce/slot-machine/master/screenshot_slot_machine.PNG)

##The Premise

Battle Slots utilises a slot machine mechanic to randomly generate a battle scenario, with varying degrees of difficulty. These are quickfire battles with high intensity.

The player begins the game by clicking the button to ‘spin’ the slot panels.  Each panel signifies a different gameplay element:

The first panel determines the level (terrain type) the player will be playing
The second panel selects a weapon for the player’s character
The third panel determines the enemy type the player will face

Once all three panels have come to a stop the level will be loaded with the appropriate results.  The player completes the level if he/she is able to defeat all enemy units while staying alive.

##The Slot Machine
The slot machine effect is simulated by manipulating the SpriteRenderer component on each of slot panels.  To begin with one random panel from each of the three sets has it’s SpriteRenderer enabled.  A time interval based on the time since the previous frame is used to determine when to rotate the slots by both enabling the next panel’s SpriteRenderer and disabling the current one.  This continues for each of the panel sets until a random-bounded time is reached.  At this point, for the first panel set, whichever panel that has it’s SpriteRenderer enabled (and thus is visible in the scene) is taken as the level result.  The remaining two panel sets continue their rotation.  Once another time interval is reached the same process occurs for the second panel set, determining the player’s weapon.  After a final interval the third and final enemy result is found.

The resultant information is then stored as a string PlayerPref so that is can be easily obtained for level generation and the character spawn in whichever scene is to be loaded.  After a short delay, SlotMachine.cs finishes by loading the appropriate level scene.

##Level Generation
The levels consist of a number of procedurally placed floating platforms and some environment hazards.  The idea was the have three levels that vary in difficulty.  The water level is relatively tame, consisting simply of a series of floating platforms.  The fire level has an added hazard in the form of a lava pit that will kill anything that comes into contact with it.  The earth level contains a similar spiked ‘death zone’ as well as containing a large single platform that pivots under weight.  This level was designed for our boss encounters.

To obtain the required randomness for the platform locations, each platform is accompanied by a sphere collider.  The platforms are placed randomly within the bounds of the scene and the physics engine is run for a number of steps.  By using this technique any of the platform spheres that are overlapping and pushed away from one another resulting in an evenly spaced out set of platforms.  The obvious downside of this technique is that platforms may get pushed outside the camera boundaries.  These platforms are simply culled.
Once all platforms have come to rest, their sphere collider is disabled and their SpriteRenderer is enabled making them visible in the scene.  At this point the player and enemies are ready to be spawned.

![alt tag](https://raw.githubusercontent.com/adamjoyce/slot-machine/master/screenshot_water.PNG)

##Player and Enemy Spawns
The player is simply spawned on one of the platforms that has already been placed in the scene.

For each platform a random float is generated between 0.0 and 1.0.  This number signifies the chance of enemies spawning on that particular platform.  That is, if the number is greater than or equal to 0.2, a single enemy will spawn on the platform.  However is the value is greater than 0.8, two enemies will spawn instead.  The game is designed so there will always be at least one enemy in each level generated.  Enemies are also unable to spawn on the same platform the player begins on.

##The Player
At the start of a level the player’s character is instantiated from a prefab.  Similarly a weapon corresponding to the result obtained from the slot machine is instantiated from a different prefab.  By having two separate prefabs for both the player character and the weapon we are able to easily incorporate more weapons into the game with relatively little fuss.  However, this technique does have it’s own shortcomings.  As the player character and the weapons are not directly linked, but instead two separate objects, there can be some issues syncing the weapon’s position with particular character animations.  This can lead to peculiar moments where the character appears to be holding the weapon in a strange position.  In hindsight it may have been sensible to instead incorporate the weapons into the character animations to achieve a more fluid feeling.

Each weapon has been designed in such a way to have its own unique strengths and weaknesses.  The gun is extremely safe to use, but it’s limited horizontal firing pattern can make it difficult to aim.  The rocket launcher on the other hand has complete freedom with its aim and can inflict a large amount of damage with it’s area of effect, but is also able to inflict damage to the player if he/she is standing too close to the blast.  Finally the knife is inherently dangerous due to the close proximity between the player and the enemy, but deals very high damage to compensate for this risk.

The player was initially designed with both double and wall jump capabilities.  As development progressed and the idea for the game formed we quickly realised that the wall jump seemed out of place within the game’s context, therefore we opted to remove this feature.

##The Enemies 
At this current stage the game contains three different enemies:

a spitter - designed to ‘spit’ fireballs at the enemy from range,
a charger - who charges at the player when they come within a certain range,
and a flyer - who swoops down and attacks from the skies.


The AI for the enemies is controlled using the RAIN AI Unity module.  While each enemy group have their own behaviour tree, all three share a common behaviour pattern.  They continuously monitor the surroundings area for the player based on their cone of vision.  Once the player is found they engage their specific aggressive behaviour, for examples a spitter would spit a fireball.  In addition, the two grounded enemies use raycasting to monitor ground ensuring that they are not about to fall from their platforms.  Similarly, flyers do the same to avoid collisions with walls.

In hindsight, it may have been more sensible to implement the AI component of the game without using the RAIN AI module.  We found that RAIN often overcomplicated the relatively simple AI we were attempting to implement, causing more problems than it was worth.  Implementing our own custom AI would have alleviated these issues but in turn required a more complicated method of detecting the player character.

##The Team
Jean-Pascal Evette - Programmer - https://github.com/JeanPascalEvette<br />
Adam Joyce - Programmer - https://github.com/adamjoyce<br />
Ciaran Wilson - Artist - https://github.com/ciaranwilson

##Youtube Demonstration Video
https://youtu.be/cMoeV2deOiI


  
