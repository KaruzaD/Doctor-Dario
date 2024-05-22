# Doctor-Dario
A 1-4 player version of the classic NES game, Dr. Mario, built from the ground up in the Unity Game Engine. 
For information on the original, see [Dr. Mario - Wikipedia](https://en.wikipedia.org/wiki/Dr._Mario).

# Game Setup Screen
- Virus Level determines how many viruses will spawn
- Speed determines how fast the pill will drop
- Snow Level effects how much the player will get snowed (only works in multiplayer). Snowing happens when an opponent makes two or more matches before spawning their next pill
  - Low will make one less block snow on your playfield
  - Med will make the normal amount of blocks snow on your playfield
  - Hi will make one extra block snow on your playfield
- Music Type determines which song will play during gameplay

# Building
This was built with Unity version 2023.2.11f1. Use a similar version to build the game. In order to export, follow instructions at 
[this link](https://www.makeuseof.com/unity-game-project-build-run/)

# Sound
I have not supplied music or sound effects, but the code is all set up for them.
- In order to have sound/music in the game, navigate to Assets/Resources/ScriptableObjects in the Editor and select the MusicClips/SFXClips resource. 
- Place music/sfx files in this folder.
- Select each corresponding track in the inspector and point it to the appropriate file.
- Test and build as normal.

# Controls
The game works with either keyboard or controllers. To play multiplayer, connect up to 3 controllers.
  Keyboard or Controller    Action
- Menu Controls:
  - W/A/S/D or D-Pad    Move the cursor
  - Enter or A    Menu forward
  - Return/Esc or B    Menu backward
  - Hold Return/Esc or B    Drop out player
  - Any Button    Add player
- Game controls
  - Enter or Start    Pause/Unpause
  - Hold Esc/Return or B while paused    Return to game setup screen
  - A/D or Left/Right    Move pill left/right
  - S or Down    Move pill down faster
  - Tab or Y    Show/Hide pill drop shadow

# Disclosure
Nearly all artwork is recreated by me from the NES game and is not my original work.

