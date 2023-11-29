Pixel Pioneers Present: Dark Ascent 

Team Members: Akhilesh Sivaganesan, Amal Chaudry, Connor Sugasawara, Kevin Kwan, Mehar Johal 

How to Play: 

Start Scene File: MainMenu.unity 

Controls/How-to-Play: 

WASD/Arrow keys to move 

Hold Right mouse to control camera (right click and drag around) 

Left Click to attack 

Ctrl to slide 

Shift to sprint 

Space to jump and double jump 

Space when close to a Wall-jumpable wall to perform Wall Jumping between walls 

Left Click when mouse cursor is visible to interact with UI elements (buttons, menus, etc.) 

General Game Idea: 

 It’s a 3D platformer, you start on the tutorial level with some pop ups to hint you on the controls and get a feel for how the game plays. There are 4 platforming levels including two levels for tutorial, a third level introducing AI enemies such as the Warden and some grunts as well as physics interactions needed to complete the level, and a fourth level implementing game hazards, as well as continuing the use of AI. Starting from the 3rd level you are chased by the Warden (better run quick). After completing 4 levels, with as many tries and checkpoint resets as you need, you are faced with the final boss fight. Attack the boss by swinging (left click or fire) to reflect projectiles back at the boss, watch out for teleportation and summoned enemies, and hit the boss 3 times to win! 

Known Problem Areas: 

Checkpoint audio retriggers, damage audio locked to animation speed, shifted player hitbox on Boss Fight Level 

 

 

 

 

Individual Team Member Contributions: 

Akhilesh Sivaganesan: 

Overview:  

Worked on UI design for the tutorial level. There is a UICanvas, comprised of several panels. These panels are activated by a script that detects entering the matching trigger zone. Since this game is primarily for mouse and keyboard, the controls featured in the tutorial involve keyboard sprites. The pause and death menus follow a simple design with the appropriate buttons, used throughout the game. Also integrated the checkpoint system developed by Kevin into the tutorial level, creating a custom emission material for those checkpoint bases. Collected textures and assets from unity asset store: JohnFarmer Keyboard Keys & Mouse sprites, LowlyPoly Rock Textures, BG Studio Free HDR Skybox, Rob Luo Stylized Lava Materials. There is a wall jump mechanic that I implemented as well. 

Script Contributions: 

CheckpointHandler.cs, CheckpointManager.cs, CheckpointTrigger.cs, DeathCollider.cs, PauseManager.cs, ProximityTrigger.cs, ResumeButtonScript.cs, QuitToMainMenu.cs, ThirdPController.cs 

Prefabs: Pause Menu.prefab, Death Screen.prefab, Tutorial Popup.prefab 

Scenes: Level1.unity 



Amal Chaudry:  

Overview: 

Worked on Level design for the demo level and Level4. Worked on elevator and moving platform implementation, allowing the player to utilize elevators to reach different areas of the level. Implemented different hazards for the player to avoid which will damage their health. Implemented HealthUI system using heart graphics which indicate the character’s health level to the player. 

Script Contributions:  

SpikeHazard.cs: Worked on spikes prefab to use as a hazard for the player, when they jump on it it will kill them instantly.  

 SwingingAxe.cs: Worked on swinging axes prefab and added functionality to damage player health by 20, animated swinging motion 

HeartUIController.cs: Implemented health UI system using heart images, each heart is 20 HP, so if player takes 20 damage they lose a heart. 

FallingRock.cs: Implemented shaking and falling rock hazard, when the player enters the trigger area, the rock will start shaking and eventually fall, meaning player must jump forward quickly or use another path to jump up. Animated shaking of the rock and implemented falling functionality. 

FallingRockController.cs: Impemented a controller for the shaking animation state machine where the transition from stable to shaking is based on a boolean flag called onRock which determines if the player is on the rock. 

ThirdPController.cs: Implemented elevator and moving platform functionality. When the player jumps on an elevator/moving platform, they trigger the event where they become a child of the elevator/moving platform so that the player and platform move together as one. 

Prefabs: Spikes.prefab, SwingingAxe.prefab, HeartContainer.prefab, FallingRock.prefab 

Scenes:  

DemoLevel.unity: Designed the demo level and served as the basis for Level1 implemented by Akhilesh 

Level4.unity: Designed and implemented Level4, including spikes, swinging axes, and falling rock hazards. In this level the player must escape from the AI Warden and also fight AI grunts. 

Imported Assets Used:  

Swinging Axe: https://sketchfab.com/3d-models/rusty-double-sided-axe-ecd44a606cb646839f66137c34ced762 

Spike Trap: https://sketchfab.com/3d-models/spike-trap-01-4022678cac214fd2963894aa152fc6f2 

Heart Graphic: https://assetstore.unity.com/packages/tools/gui/simple-heart-health-system-120676 

 

Kevin Kwan:  

3rdPPlayer.prefab, ThirdPController.cs: Worked on implementing basic character controller script featuring walking, running, and jumping while allowing for smooth, responsive input. Also implemented 3rd Person Camera control similar to Roblox’s (holding down right click and moving the mouse around). Used https://assetstore.unity.com/packages/3d/characters/little-ghost-free-229325 to serve as our player model. Used Autodesk Maya 2024 and keyframe animation to create custom animations for our character such as running, sliding, jumping, falling. Then, created Mechanim animation state machines, animator layers, and blend trees to blend the movement animations together for our player. The ThirdPController.cs was changed to account for this. Animations implemented include walking, running, sliding, jumping, falling, attacking, taking damage, and dying. Added functionality for the player to visually attack, take damage, and also die as well. 

CheckHandler.prefab, CheckpointHandler.cs, ExampleCheckpoint.prefab, ExampleWardenSpawnpoint.prefab: Created the CheckpointHandler for each level to use to handle the loading and storage of player's checkpoints in the game. The player will start at a spawn checkpoint and can reach multiple checkpoints before reaching the final checkpoint to progress to the next level. The CheckpointHandler.cs uses PlayerPreferences store the current/latest level and current/latest checkpoint that the player has reached and updates these values accordingly. The handler is dynamic, so developers can add and remove intermediate checkpoints as needed using ExampleCheckpoint.prefab. Respawn player functionality has also been implemented to respawn the player at the latest checkpoint reached if they die or decide to restart. Respawn Warden has also been implemented to respawn the Warden at its own spawnpoint(s) behind the player’s checkpoints (to prevent spawncamping) if the level has a Warden. 

MainMenu.unity (scene), MainMenuScript.cs, VolumeHandler.cs: Created the Main Menu to the game. Players are greeted by their player model jumping across a gap with the title of the game and buttons to click. The buttons include “Start New Game” (displays as “Continue Game” if the player has played the game before and hit checkpoints in levels which were saved in PlayerPreferences from CheckpointHandler.cs), “Settings” so the player can change the volume of the game and return back to the main menu, “Credits” so the player can view all the models, assets, sounds, etc. That we have used in the game, and “Quit Game” where the player can confirm to close the game or return to the main menu. The MainMenuScript.cs handles all of these button interactions and the hiding/showing of these different panels/pages that were mentioned earlier. Worked on the basic first iteration for volume handler script to adjust the global volume of AudioListener through the use of a slider on the main menu settings page. Whenever the value of the slider changes, the volume is changed and the value is stored in PlayerPreferences. So, next time whenever the player loads the game again or loads a level/scene, the script loads back the saved data so the volume level persists across levels. 

Level3.unity (scene), LavaPool.prefab, Pushable.cs, LavaHitbox.cs, ActivationCollector.cs: Did level design for Level3. Level3 is the first level to introduce the AI Warden that will chase the player. It also has some grunts for the player to fight. The player will perform some platforming, use physics to push a rock (Pushable.cs) into a hole (ActivationCollector.cs) in order to activate an elevator to reach a checkpoint and more platforms above. After some platforming, the player will use physics again to push a rock through a corridor to get through it and reach the end checkpoint. In this level, I used an existing Lava Pool model with materials and shaders to represent the Lava and using Trigger and LavaHitbox.cs, we would kill the player if they fell off of the platforms into the lava. 



Connor Sugasawara:  

ThirdPPlayer.prefab, ThirdPController.cs: Implemented sliding for the player character. Sliding lowers the player’s hitbox and gradually reduces their speed. Tested a version of sliding that provides a temporary boost of speed for more advanced movement/platforming options (currently not implemented). Updated player input to use Unity’s Input Manager (unable to test but hopefully allowing for controller support). Added audio to 3rdPPlayer.prefab and implemented appropriate and unique audio cues for walking, running, jumping, wall jumping, sliding, attacking, taking damage, and dying. 

MainMenuScript.cs, VolumeHandler.cs, GameAudio.mixer: Implemented controls for master volume, music volume, and sound effect volume using Unity’s AudioMixer, all of which individually persist via PlayerPreferences. Added controls in the settings menu to reflect this. Also added a reset progress button to clear existing level progression in the settings menu. Added audio cues for interacting with UI elements, updating MainMenuScene.unity, Pause Menu.prefab and Death Screen.prefab to reflect this change. 

CheckpointReached.cs, CheckpointHandler.cs: Added audio for reaching a checkpoint, updating ExampleCheckpoint.prefab to reflect this change. Updated respawn behavior to use Unity’s Input Manager. 

Level2.unity (scene): Did level design for Level2. Level2 introduces enemy grunts and shows the player how to control the camera and attack enemies. Updated moving platform animation and tutorial popups to work in the context of Level2. Also retooled and repurposed other assets created and gathered by teammates in this level for consistency. 

Created original music for the game using FL Studio. All sound effect audio mentioned above was selected from https://assetstore.unity.com/packages/audio/sound-fx/universal-sound-fx-17256, which I already owned from a prior independent project. 

 

Mehar Johal: 

Implemented the AI for the game, from the enemies to the boss fight as well as polish on their sound effects, particles, and animations. Added cutscenes to the game introducing the warden initially, and then the boss fight as well. Also implemented player attacking and enemies dying, along with particles associated. Animation controlling logic done as well as particles and audio, but animations, models, audio fx, and some particles utilized asset store materials, and credited as such in the game. Made the scene as well for the boss fight (level 5) 

The enemies are as follows: 

Warden: A demonic figure who chases you starting level 3, he can fly through obstacles and will chase the player until the level is over, swinging his arms to attack if they get close. Only melee 

Frog Grunt: A red frog whose primary attack is a jump lunge. When a player enters aggro range, the frog grunt leaps towards the player, snarls, and tries to take a bite out of them. 

Rock Frog Grunt: A green frog who has two attack modes. One attack mode is when rocks are placed nearby, the rock frog grunt looks for nearby rocks, picks them up and shoots them at the player. If no more rocks are available, it resorts to lumbering towards the player and biting them 

Bat Grunt: A flying enemy who shoots projectiles at the player, hovering and flapping annoyingly around them as well. Very persistent with large aggro range. 

Final Boss: The final warden boss detailed a bit above with some interesting teleportation and fun mechanics for you, the player to figure out. Can summon frog grunts as well to assist when it gets injured. 

Scripts Implemented:  

BatGrunt.cs, BossIntroCutscene.cs, BossProjectile.cs, CutScene.cs, FInalBoss.cs, FinalCutScene.cs, FrogGrunt.cs, Health.cs, PlayerAttack.cs, Projectile.cs, RockFrogGrunt.cs,  ThirdPController.cs (did double jumping for the player) 

Prefabs:  

Bat Grunt, Boss TP Particle, CutScene_Warden, Final Boss Projectile, Final Boss.prfab, Frog Grunt, Rock Frog Grunt, Projectile, Thrown Rock, Warden 

Scenes: WinCutscene, Level5, BossFightIntro, WardenCutscene 