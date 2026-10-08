# AR Labs

A mobile AR app built in Unity as part of a series of lab assignments: Pudges on real-world surfaces, gestures, merging, video on recognized images, face tracking and light estimation.

**[Download APK](https://github.com/splittech/ar-labs/releases/latest)**

## Contents

- [Stack](#stack)
- [Features](#features)
  - [Create Mode](#create-mode)
  - [Merging](#merging)
  - [Edit Mode](#edit-mode)
  - [Image and Face Tracking](#image-and-face-tracking)
  - [Light Estimation](#light-estimation)
- [Architecture](#architecture)
- [Tests](#tests)
- [Controls](#controls)
- [Getting Started](#getting-started)
- [Third-Party Content](#third-party-content)

## Stack

- **Unity** 6000.3.10f1
- **AR Foundation** 6.5, ARCore, ARKit, XR Simulation
- **VContainer** — dependency injection
- **R3** — events
- **DOTween** — animations
- **Lean Touch** — gesture recognition
- **NUnit**, **NSubstitute** — tests

## Features

| Lab | Contents                                                                      |
| --- | ----------------------------------------------------------------------------- |
| 1   | Plane detection, Pudge spawning, animations                                   |
| 2   | Menus and game modes, spawn marker, Pudge merging, FPS counter                |
| 3   | Edit mode: Pudge selection, info panel, scaling, rotation, reset              |
| 4   | Gestures: swipe to rotate and cross to delete, logging service, despawn fade  |
| 5   | Image tracking with a video player, face tracking                             |
| 6   | Light estimation                                                              |
| —   | EditMode tests for the core logic                                             |

### Create Mode

Pick a Pudge type (normal, happy or sad), press on the floor and, without lifting your finger, drag the marker to the spot you want. The Pudge appears where you release your finger.

### Merging

Two Pudges of the same type and size walk towards each other and merge into a bigger one. Once the size reaches the threshold, a final effect plays instead of a new Pudge appearing. Merging starts when a new Pudge is spawned and when a Pudge is deselected in edit mode.

### Edit Mode

Tap a Pudge to select it. The panel at the bottom shows its name, description and current changes. The buttons let you change the scale, rotate the Pudge by a fixed angle and reset the changes.

A horizontal swipe rotates the selected Pudge, and a cross drawn over another Pudge deletes it.

### Image and Face Tracking

A video player with playback controls appears on a recognized image from the library.

In face tracking mode the app switches to the front camera and shows an object on the detected face.

### Light Estimation

The direction, brightness and color of the main light in the scene, as well as the ambient light, adapt to the real-world lighting. What's available depends on the platform: on ARCore full HDR estimation is supported for the rear camera, on ARKit — for the front camera.

## Architecture

The code lives in `Assets/Game/Scripts` and is split into three assemblies:

- **`Game.Core`** — services that don't depend on the game: input, gestures, raycasts, AR, light estimation, timers, logging.
- **`Game.Gameplay`** — Pudges, their spawning, editing and merging, game modes.
- **`Game.Menu`** — menus and switching between them.

Each feature consists of a logic class and a MonoBehaviour View that deals with Unity: transforms, prefabs, UI. The logic depends on the View only through an interface (for example, `Pudge` → `IPudgeView`), so it can be tested without a scene. Dependencies are wired by VContainer DI scopes (`CoreScope`, `MenuScope`, `GameplayScope`), and services are enabled at startup by bootstrappers.

Game modes (`EmptyGameMode`, `CreateGameMode`, `EditGameMode`, `ImageTrackingMode`, `FaceTrackingMode`) switch together with the menu through `GameModeSwitcher`.

## Tests

`Assets/Game/Tests/Edit Mode` contains 115 EditMode tests for the core logic. They cover Pudges and their transformations, spawning, the spawn marker, merging, editing, gesture recognition, the timer, and switching menus and game modes.

The tests create objects through factories in `Setup.cs`, and dependencies are replaced with NSubstitute mocks through interfaces.

## Controls

| Gesture                          | Mode   | Action                                                                     |
| -------------------------------- | ------ | -------------------------------------------------------------------------- |
| Press and drag on the floor      | Create | Place and move the spawn marker                                            |
| Release the finger               | Create | Spawn a Pudge at the marker                                                |
| Tap a Pudge                      | Edit   | Select the Pudge                                                           |
| Tap an empty spot                | Edit   | Deselect                                                                   |
| Horizontal swipe                 | Edit   | Rotate the selected Pudge: a swipe across the full screen width is a full turn |
| Cross made of two diagonal swipes | Edit  | Delete the unselected Pudge under the center of the cross                  |

## Getting Started

1. Install Unity **6000.3.10f1** via Unity Hub with the Android Build Support module (and iOS Build Support if you build for iOS).
2. Clone the repository. Binary files are stored with Git LFS:
   ```bash
   git lfs install
   git clone https://github.com/splittech/ar-labs.git
   ```
3. Open the project in Unity Hub. Third-party packages are fetched through the Package Manager and NuGetForUnity.
4. Open the `Assets/Game/Scenes/CoreScene.unity` scene.
5. **In the editor** you can run the app with XR Simulation. Light estimation doesn't work in the simulation and has to be checked on a device.
6. **On a phone:** select the Android platform in File → Build Profiles and build an APK. You need a device that supports ARCore.

## Third-Party Content

- Lean Touch and DOTween are distributed under their own licenses.
- Pudge models and likeness belong to Valve (Dota 2).
- The videos in `Assets/Game/Content/Videos` are taken from public sources and used for educational purposes.
