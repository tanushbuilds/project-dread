# Project Dread

> *Navigate an interactive flat, eat, wait, and slowly realise something is wrong — through atmosphere and physiological mechanics.*
> 
---

## Overview

**Project Dread** is a first-person psychological horror game built in Unity. It forgoes conventional horror tropes in favour of something quieter and more suffocating — dread built from the ordinary. The player comes home, heats up food, eats on the sofa. Then someone knocks.

The game draws its tension from the mundane. Every interaction — opening the fridge, waiting by the microwave, sitting down to eat — is engineered to establish normalcy before eroding it. The horror does not announce itself. It lingers at the end of a hallway. It knocks politely.

---

## Gameplay

Project Dread is a narrative-driven, first-person experience set entirely within a residential apartment building. There is no combat, no HUD, and no explicit objectives. The player inhabits a character and lives a few quiet minutes of their life — until those minutes stop feeling quiet.

**Current narrative sequence:**

1. **Arrival** — The player enters their flat in a still, dimly lit apartment building.
2. **Food Preparation** — Food is retrieved from the fridge, placed in the microwave, and heated while the player waits.
3. **Dinner** — The player sits on the sofa and eats. The flat is calm. Ordinary. Safe.
4. **First Knock** — The door is answered. The hallway is empty, except for a figure at the far end — standing, watching. He turns and walks away.
5. **Bathroom Break** — The player uses the toilet. Life, for a moment, continues as normal.
6. **Second Knock** — A neighbour. They warn the player about a strange man roaming the building.

The stage is set. Something has begun.

---

## Systems & Features

| System | Description |
|---|---|
| **First-Person Controller** | Smooth, grounded movement with responsive look input |
| **Food Consumption System** | Item-based food pickup and consumption with interaction state tracking |
| **Dialogue System** | Event and proximity-triggered NPC dialogue sequences |
| **Appliance Interaction System** | Microwave simulation — place food, initiate heating cycle, await completion, retrieve item |
| **Seated Interaction System** | Context-aware sofa seating with sit and stand state transitions |
| **Bathroom Interaction System** | Toilet interaction with timed physiological sequence — a deliberate grounding mechanic |
| **Baked Lighting & Lightmapping** | Pre-baked Unity lightmaps delivering a warm, oppressive interior atmosphere with deep shadow contrast |
| **Custom Shader Pipeline** | Hand-authored ShaderLab and HLSL shaders for material fidelity and atmosphere control |

---

## Getting Started

### Prerequisites

- **Unity 2022.3 LTS** or newer
- **Git LFS** installed (required for binary assets)

### Installation

```bash
# Clone the repository
git clone https://github.com/tanushbuilds/project-dread.git

# Navigate into the project directory
cd project-dread
```

1. Open **Unity Hub**
2. Select **Add project from disk**
3. Point it to the cloned `project-dread` folder
4. Open the project and load the main scene from `Assets/Scenes/`
5. Press **Play**

---

## Roadmap

- [x] First-Person Controller
- [x] Food Consumption System
- [x] Dialogue System
- [x] Appliance Interaction System (Microwave)
- [x] Seated Interaction System (Sofa)
- [x] Bathroom Interaction System
- [x] Baked Lighting & Lightmapping
- [x] Stranger NPC — Hallway Event
- [x] Neighbour NPC — Warning Dialogue
- [ ] Post-Processing & Visual Atmosphere Pass

---

## Author

**tanushbuilds** — [github.com/tanushbuilds](https://github.com/tanushbuilds)

---

<p align="center">This project is actively in development. Features, systems, and content are subject to change as production continues.</p>
