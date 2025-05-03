# Unity Audio Scene

![image](https://github.com/user-attachments/assets/592cb1fa-81c2-4094-a427-bea2547ffe13)

An immersive 3D environment showcasing advanced Unity audio implementation techniques with spatial audio, dynamic music transitions, and responsive sound design.

## Overview

This project demonstrates professional audio implementation techniques in Unity, focusing on creating a believable and immersive soundscape for 3D environments. The project features dynamic surface-based footsteps, music zone transitions, cave reverb effects, and a complete audio mixer system.

## Features

### Audio Mixer System
- Complete mixer architecture with separate channels for:
  - Master volume
  - Music
  - SFX
  - Ambience
- Volume control for all channels via in-game options menu
- Audio snapshots for different environments (default and cave)

### Surface-Based Footsteps
- Dynamic footstep sounds based on terrain textures
- Support for multiple surface types:
  - Grass
  - Dirt
  - Sand
  - Rock
  - Water
- Natural variation through randomized pitch and volume

### Music System
- Seamless music zone transitions with crossfading
- Distinct music for different areas
- Global music controller with custom transition timing
- Volume control and persistence across scenes

### Environment Effects
- Cave audio transitions with reverb effect
- Ambient sound placement for consistent environment audio
- 3D spatialized audio for increased immersion

### Menu System
- Complete main menu with:
  - Title display
  - Play button
  - Options menu
  - Credits screen
  - Exit functionality
- In-game pause menu with:
  - Resume
  - Options
  - Return to main menu
  - Exit game

## Scripts Overview

- `PlayFootstepOnSurface.cs`: Handles dynamic footstep sounds based on terrain textures
- `CaveAudioTransition.cs`: Manages audio snapshot transitions for cave environments
- `MusicZoneTrigger.cs`: Controls crossfading between different music tracks
- `OptionsMenuController.cs`: Manages audio settings and player preferences
- `MainMenuController.cs`: Handles the main menu functionality
- `PauseMenuController.cs`: Controls the in-game pause menu

## License

MIT License - See LICENSE file for details
