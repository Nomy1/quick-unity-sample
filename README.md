# Quick Unity Sample

## Author

Jonathan Bro

## Installation

Unity3D 2022.2.1f1

## Overview

This is a tic-tac-toe clone made in roughly 4 hours.

### Development

The game begins with the Bootloader scene which stays alive during the duration of the game. It's intended for logic which needs to survive scene loads.

I decided to create the game using liberal use of async await since tic-tac-toe is a simple game and the number of player interactions can be easily grasped.

I have player configuration scriptable objects which hold player information and is injected into the game screen. These would be created, serialized, and saved to disk (or online) in a real world scenario.

It can get confusing if saving game state values directly to PlayerPrefs, so instead data is serialized/deserialized in JSON format since it's simple. 

I was able to add some simple animations when selecting tiles since this is what the player spends most of the time doing. I would have loved to add SFX and a background as well, but I wanted to keep development time to a minimum.

### How to Play

Each player takes turn selecting tiles on the screen. The first player with three O or X in a row wins!

### 3rd Party Plugins

- DOTween 1.2.705
- UniTask 2.3.2