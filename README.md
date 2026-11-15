# Geometry Dash VR

An unofficial, original VR rhythm-platformer project targeting Meta Quest 3.

## Project status

**Prototype foundation — not yet a playable game.** This repository is the dedicated home for all Geometry Dash VR development. It is separate from the Geode Versus Mode mod.

## Technology

- Unity 6 (6000.0)
- OpenXR
- Unity XR Plug-in Management
- Unity Input System
- Android / Meta Quest 3 target

## First milestone: VR foundation

- [x] Create the Unity project structure and package manifest
- [x] Add a tracked-head/controller rig foundation
- [x] Add an experimental hand-contact locomotion prototype
- [ ] Create and configure the first Unity scene
- [ ] Install Unity and Android build modules, then validate a Quest build
- [ ] Test on the physical Quest 3

## Open the project

1. Install Unity Hub and a Unity 6.0 editor.
2. Add the **Android Build Support**, Android SDK & NDK Tools, and OpenJDK modules.
3. Open this repository's root folder in Unity Hub.
4. Let Unity resolve packages.
5. Create a scene at `Assets/Scenes/VRPrototype.unity`.
6. Add an XR Origin with a tracked camera and left/right controller transforms.
7. Add a CharacterController to the rig root and attach `HandPushLocomotion`.
8. Assign the camera and controller transforms in the Inspector.
9. Configure an Android build with OpenXR enabled and the Quest-compatible Android settings.

## Locomotion prototype

The initial hand-push script is an experimental starting point, not a complete Gorilla Tag implementation. It uses tracked hand movement while the grip button is held and the hand is touching a surface. Comfort options and collision testing are required before treating it as production-ready.

## Important project boundaries

- Use original code, art, music, levels, and branding.
- Do not bundle proprietary Geometry Dash or RobTop game files.
- Any third-party code adopted later must retain its license and attribution.
- Globed cross-play is a future research goal, not implemented.

## Planned milestones

1. Quest 3 VR rig and comfortable locomotion.
2. Walkable 3D headquarters and room portals.
3. Rhythm gameplay and original prototype levels.
4. Full level editor.
5. Versus modes and multiplayer.
6. Globed interoperability research.
7. Quest performance, accessibility, and release testing.
