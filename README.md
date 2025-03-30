# SquidGame
This repository contains a short video game inspired by the popular show "Squid Game." The game comprises three mini-games, two of which utilize face tracking to control the characters, providing an immersive and interactive experience.

## Overview

The **SquidGame** project is a collection of three engaging mini-games that challenge players' reflexes and strategic thinking. By incorporating face tracking technology, the game offers a unique and interactive gameplay experience, allowing players to control their character through facial movements.

## Mini-Games

1. **Mini-Game 1 (Red-Light Green-Light):**
   - *Description:* In this mini-game the goal is to get to the end line of the field without being seen moving and before the timer runs out.
   - *Face Tracking:* The game tracks the openness of the user's eyes to control if the player is moving or not, utilizing sound cues to indicate if they can move or not. If the player's eyes register as open, the player's character runs forward and if they register as closed the character stands still.

2. **Mini-Game 2 (Dalgona):**
   - *Description:* The goal of the game is to carve out the shape provided on the cookie using the mouse, without touching any part of the cookie except the indicated lines (that breaks the cookie and the player loses).
   - *Face Tracking:* This is the only mini-game that doesn't utilize face tracking.

3. **Mini-Game 3 (The Glass Tile Game):**
   - *Description:* The player's goal is to pick one of the two provided glass tiles, one of which is real and one of which breaks upon contact making the player fall through. The player repeats the process untill they reach the end of the course or lose all their lives.
   - *Face Tracking:* The game tracks the position of the player's head. Tilting the head to the left selects the left tile and tilting it to the right selects the right one (the selected tile is highlighted). When the player is confident in their decision, they nod upwards with their head to confirm their choice and jump onto the selected tile.

## Installation

1. **Clone the Repository:**
   
    ```bash
   git clone https://github.com/BMajeric/SquidGame.git
    ```
2. Navigate to the Project Directory:
   
   ```
   cd SquidGame
   ```
3. Open in Unity:
   - Ensure you habe Unity intalled (recommended version: 2021.3 LTS or newer)
   - Open the project using Unity Hub
4. Run the Game:
   - Navigate to the ```MainMenu``` scene and press the ```Play``` button
   - OR build the game making sure the ```MainMenu``` scene is included in the scenes list and is placed at the top of the list

## Future Improvements
- Enhanced Face Tracking Integration:
  - Improve the accuracy and responsiveness of face tracking controls to provide a smoother and more satisfying gameplay experience
- UI/UX Enhancements:
  - Improve the user interface and overall user experience to make the game more intuitive and engaging
  - Add a better tutorial and player instructions to ensure players know how to play the game correctly
