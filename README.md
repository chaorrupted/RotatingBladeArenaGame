# RotatingBladeArenaGame
Code for a simple survival arena game with the core mechanic of melee weapons rotating around characters, similar to Vampire Survivors.

# Features
- move your character around the arena. You will not be able to move outside of the level bounds.
  - camera will be centered on your character.
- collect weapon pickups to add swords to your weapon belt
  - pickups will randomly spawn all around the arena
- clash with other enemies who also have swords
  - clashing swords fly off the screen
- somewhat smart enemy AI will make the gameplay interesting and fun
  - defeated enemies drop their weapons as pickups
- extendable pool manager and pooling for swords and pickups
- extendable character classes
- utilities for pickup magnet, game over screens, and various other misc. game tasks

# About scracthcard:
The original project also had this scratchcard package: https://assetstore.unity.com/packages/tools/particles-effects/scratch-card-228309 which I modified so that characters and weapons will scratch the 2-layer level background. I'm not publicly sharing these modifications since it would require that I share the source code for this paid asset, and I don't think I'm allowed to do that. If you already paid for this package and are interested in modifying it to scratch using game objects rather than user inputs, you may or may not be able to contact me and ask and I may or may not be able to help you.

# how to run the game:
You'll want to create some required gameobjects and prefabs and arrange the scene as described in this section to be able to build/play the game. You should be able to make out which script goes where and which serialize field is assigned where with some trial and error. Or, you may or may not be able to contact me and ask and I may or may not be able to help you.

scene layout:
<img width="289" alt="Screenshot 2025-05-09 at 15 53 57" src="https://github.com/user-attachments/assets/aa26406d-f775-4a3a-a904-d548bc5e78ba" />

player prefab: (enemy prefab is the same except it has the enemy script instead of player)
<img width="212" alt="Screenshot 2025-05-09 at 15 55 42" src="https://github.com/user-attachments/assets/686ff3ff-dd49-4858-83b0-013a71b612d3" />


You may need to delete/comment out logic related to scratchcarding from character class.
