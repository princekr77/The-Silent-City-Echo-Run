# The Silent City: Echo Run

A 3D endless runner set in a city frozen in time after a mysterious meteor event.

## 🎮 Game Overview

Frozen Loop Runner is a 3D endless runner developed in Unity using the Built-in Render Pipeline.
After a meteor event freezes an entire city in time, the player is trapped in an infinite loop,
running endlessly through the same city while avoiding frozen obstacles.

The city itself is not destroyed — instead, time has stopped mid-moment, creating
unique and unpredictable obstacles inside a looping environment.

## 🧠 Core Gameplay Mechanics

- Automatic forward running with gradually increasing speed
- Lane-based movement system (Left / Middle / Right)
- Jump and Slide mechanics for obstacle avoidance
- City tiles loop infinitely to create an endless environment
- Obstacles are embedded directly inside city tiles
- Each time a tile loops, obstacles are refreshed randomly

## 🎮 Controls

### PC
- A / Left Arrow → Move Left
- D / Right Arrow → Move Right
- Space → Jump
- S / Down Arrow → Slide

### Android
- Swipe Left / Right → Change Lane
- Swipe Up → Jump
- Swipe Down → Slide

## Level & Obstacle Design

- The city is built using reusable tile prefabs
- Each tile contains predefined obstacle spawn positions
- Obstacles are part of tile prefabs (not spawned globally)
- When a tile loops to the end, its obstacles are replaced randomly
- This approach ensures:
  - Consistent spacing
  - No overlapping obstacles
  - Stable performance on mobile devices

