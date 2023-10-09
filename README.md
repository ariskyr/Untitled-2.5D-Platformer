# Untitled 2.5 Platformer

## Overview
A 2.5D game that is a work in progress

## Table of Contents

- [Game Concept](#game-concept)
- [Gameplay](#gameplay)
- [Features](#features)
- [Game Mechanics](#game-mechanics)
- [Artwork](#artwork)
- [Art Direction](#art_direction)
- [Sound and Music](#sound-and-music)
- [Platform and Technology](#platform-and-technology)
- [Controls](#controls)
- [Installation](#installation)

## Game Concept

Reimagining an old game with modern gameplay mechanics using the Unity Engine. 
 1. [Placeholder: Provide Story/Setting]
 2. [Placeholder: whats the player's goal]
 3. [Placeholder: differences from the old game, improvements etc]

## Gameplay

 [Placeholder: Provide how the player progresses, level design etc]

## Features

 [Placeholder: provide any features that could/will exist]

## Game Mechanics

 **Movement** is done in a 2-axis fashion using front-back / left-right animations. There
 is support for jumping/crouching so we have to implement those in-game in some manner on the
 level design.
 **Combat** is done probably using left click to attack for now. Future enhancements could be dodging
 blocking etc...

## Artwork
 Primarily made by us

 1. Player is using animations from [mystic_woods](https://game-endeavor.itch.io/mystic-woods) as a 
 placeholder for now. We will be using a different character probably.

 2. **Modular Interior House Pack**: 105 Low Poly assets for primary use in interior spaces. All assets are made to be modular in 2x2m tile.
    * 5 Wooden Beams
    * Flooring + 15 different Wall Models (Wood + Stone)
    * 3 Wardrobe Assets, 1 bed, 1 pillow, 2 sacks
    * Torch, 2 lanterns and 3 candles
    * 2 Slim (1m width) Wooden walls + 3 Short (0.25m height) wooden walls
    * 2 carpets, 1 door, 1 wooden railing
    * Fireplace + 2 lumber assets
    * 2 Hanging Cloths + Curtains
    * 4 different cupboards + 2 jugs, 2 plates, 3 cups, 3 bottles, 3 bowls, 4 pots + leaves
    * 2 Windows, 3 ladders, 1 shed
    * 1 chest, 1 crate, 2 barrels, 1 basket
    * 2 tables, 2 chairs, 3 stools, 2 bars
    * 10 different book assets, (single, stacks and multiple)

3. **Modular Exterior House Pack**: TODO (mostly stuff for homes/towns)
4. **Modular Environment Pack**: Trees, Stones, Cliffs etc (will be used for overworld and caves/beaches)

## Art Direction

* **SreenSpace Cavity**:
A ScreenSpace cavity renderer feature. Allows highlighting of mesh edges to make assets pop a bit more (since its low poly). Replicates what blender cavity option does.
    1. Main renderer feature is in ScreenSpaceCavity/Shaders/**Cavity**. Place onto the URP Renderer Data (options to configure in there)
    2. Custom **ToonyLight** shader in ShaderPack/Shaders that can be used on any asset to enable the effect.

* **Mixed GI**:
    * Bake Lightmaps for each scene. Set static all object that are not movable and generate the lightmap. Settings can be tweaked.

    * a Light probe group is used to sample the lightmap on different points and display that on dynamic objects. WIP: Still need to apply that to 2D sprites.

* **Post-Process**:
    * Each scene contains a Global_PPV (Global volume with a post process profile slapped on). All Post process settings are contained there (Tonemapping, Bloom etc...)

    * For specific scenarios a Local_PPV can be created to make custom setups for different areas.

## Sound and Music

 [Placeholder: idk, will see]

## Platform and Technology

 * Unity Engine
 * Windows

## Controls

 Standard Inputs used by most games, Scheme can be changed from the options (in the future)
 For now:
   1. WASD -> Movement
   2. Left click -> Attack
   3. E -> Interact

## Installation

 [Placeholder: will see]

## System Explanation

* **Main Camera**:
    1. <u>CameraFollow.cs</u> -> camera follows the player at a fixed point

* **Player**:
    1. <u>PlayerController.cs</u> -> controls how the player falls, what is ground etc.. (Brackeys)
    2. <u>PlayerMovement.cs</u> -> attack range, movement speed, attack damage... stuff like that (Brackeys)
    3. <u>Interactor.cs</u> -> handles when the InteractionCanvas is shown if an interactable object is near.
    4. <u>CircleWipeTransition.cs</u> -> handles scene transition using a custom HLSL shader (SG_MaskTransition),
    invoke by calling the StartTransition method with a string of the level name.

* **Interactables**:
Could be stuff like doors, tables etc... depending on the scope the game will take. All interactable objects
implement the interactable interface to get the prompt texts and the action.
    1. <u>Door.cs</u> -> press e to open a door and trigger a scene transition to an interior level