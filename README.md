# Tetris

## Aim of the project:
The aim of the project was to practise C# and create fully functional GUI game with music.

## About
Project has initial pixel art welcome screen made out from real Prague photo. This was done by my software which can be found in other repository.

At settings screen, player has an option to choose at which level he wants to start. The higher the level, the more score you get for cleared
lines but also the higher the difficulty.
There is also possibility to turn ON or OFF background music. There are 3 original NES tetris soundtracks.

During gameplay player's goal is to get high-score by clearing lines. The more lines are cleared at the same time, the higher reward.
Ideally, player wants to score Tetrises - 4 lines cleared at the same time.
Interactive sounds, when moving tetrominos around, are also included in the game.

If the player reaches high-score limit of 10_000 points, his achievement is recoreded at score screen and player can sign the record
with his name.

Game then continues again from settings menu.

## Controls:
← ↑ ↓ →   - Navigation__
X Z       - Rotation__
Enter     - Confirm__
Esc       - Exit application

## Status of the project:
Functional

## Bugs:
These bugs are occasional. Might not happen at all if a person does not try to destroy the application.

- There is a merge of tetrominos when tetromino moves diagonally down. Happens both at slow and fast tetromino movement (↓ pressed).
- Rotation of tetromino out of side and bottom boundaries.

![Image](Praha.png)
![Image](Settings.JPG)
![Image](InGame1.JPG)
![Image](Score.JPG)
