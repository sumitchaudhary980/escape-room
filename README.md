# 🔐 Escape Room — Unity 3D Game

A first-person 3D escape room game built in Unity where players must solve puzzles in three uniquely themed rooms to escape before the timer runs out.

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)
![License](https://img.shields.io/badge/License-MIT-green)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac%20%7C%20Linux-blue)![Pipeline](https://img.shields.io/badge/Render%20Pipeline-URP-purple)

---

## 🎮 About the Game

**Escape Room** is a beginner Unity 3D project featuring three fully designed rooms — a Living Room, a Kitchen, and a Bathroom all combined i  one room and 2 DLc's — each with a unique puzzle that must be solved to unlock the door and progress to the next room. A countdown timer adds pressure to every room. Run out of time and you lose. Solve all three puzzles and you escape!

---

## 🕹️ Gameplay

- **Room 1 — Apartment**: Find a hidden note in all the rooms and combining the first digit answer of all rooms is answer of main door.
- **Room 2 — DLC**: Explore the room and rotate the painting in certain position to open the door.
- **Room 3 — DLC**: Not decide yet!

---

## ✨ Features

- 🏠 Three fully designed themed rooms — Apartment and 2 DLC
- ⏱️ Countdown timer per room — escape before time runs out
- 🔓 Unique puzzle mechanic in every room
- 🚪 Door unlock animation on puzzle completion
- 🏆 Win screen showing your escape time
- 💀 Lose screen with play again option
- 🔊 Ambient sound and background music per room
- 🎨 Built with Unity URP for modern visuals

---

## 🛠️ Built With

- **Engine**: Unity 6000.0.58f2 LTS
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Language**: C#
- **Version Control**: Git

### Assets Used
- [Apartment Kit](https://assetstore.unity.com/packages/3d/environments/apartment-kit-124055) — Room structure, furniture, props
- [Keypad Free](https://assetstore.unity.com/packages/3d/props/electronics/keypad-free-262151) — Keypad prop for Room 1

---

## 📁 Project Structure

```
Assets/
  ├── Scenes/
  │     ├── Room1.unity        ← Living Room
  │     ├── Room2.unity        ← Kitchen
  │     ├── Room3.unity        ← Bathroom
  │     └── WinScreen.unity    ← Win screen
  ├── Scripts/
  │     ├── GameManager.cs     ← Core game state
  │     ├── RoomLoader.cs      ← Scene transitions
  │     ├── TimerManager.cs    ← Countdown timer
  │     ├── KeypadPuzzle.cs    ← Room 1 puzzle
  │     ├── InventorySystem.cs ← Item pickup & combine
  │     └── SwitchPuzzle.cs    ← Room 3 puzzle
  ├── Prefabs/
  ├── Materials/
  └── Audio/
```

---

## 🚀 Getting Started

### Prerequisites
- Unity 6000.0.58f2
- Universal Render Pipeline (URP) package
- Git

### Installation

1. Clone the repository
```bash
git clone https://github.com/sumitchaudhary980/escape-room-unity.git
```

2. Open Unity Hub → Click **Add** → Select the cloned folder

3. Open the project in Unity 2022.3+

4. Open the starting scene
```
File → Open Scene → Assets/Scenes/Room1
```

5. Press **Play** to run the game

---

## 🎯 How to Play

| Action | Control |
|--------|---------|
| Move | WASD |
| Look around | Mouse |
| Interact | Left Click | E
| Pause | Escape |

---

## 📸 Screenshots

> Screenshots coming soon as development progresses

---


## 🐛 Known Issues

- Materials may appear pink on first import — run Edit → Rendering → Materials → Convert All to URP to fix

---

## 🤝 Contributing

This is a personal beginner Unity project. Feedback and suggestions are welcome! Feel free to open an issue or submit a pull request.

---

## 📄 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

## 👤 Author

**Sumit Kumar Chaudhary**
- GitHub: [@sumitchaudhary980](https://github.com/sumitchaudhary980)

---

## 🙏 Acknowledgements

- Unity Technologies for the engine and free learning resources
- Brick Project Studio for the Apartment Kit asset
- Unity Asset Store community for free assets

---

> Made with ❤️ as a beginner Unity 3D project
