```
  >__,,  _____ __ __ _____ _____ 
  { '<'}|     |  |  |   __| __  |
 /_)__} |  |  |  |  |   __|    -(
 __"_"__\_____/\___/|_____|__|\__\
| _   |     |  _  | __  |     \
| _  <|  |  |     |    -(  |  |
\_____\_____|__|__|__|\__\____/
```

# Overboard 🏴‍☠️
*"The floor is lava… but in a shipwreck!"*

![player jumps between platforms at Imagine RIT](/overboard-in-action.jpg)

Overboard is a **time-based "floor is lava" survival game** where shipwrecked pirates must navigate a hazardous arena, collect doubloons, and avoid peril before the rescue boat arrives. The game takes place in a **10' x 6' play area** using **top-down floor and wall projections**, with player movement tracked via a **Kinect v2 sensor**. The game is built in **Unity 6** for interactive projections and UI.

---

## 🛠️ Installation & Setup

### Prerequisites
- [**Unity 6** (version 6000.0.37f1)](https://unity.com/releases/unity-6-releases)
- **Windows** device
- [**Kinect for Windows SDK 2.0**](https://www.microsoft.com/en-us/download/details.aspx?id=44561)

### Steps
1. Install Kinect for Windows SDK 2.0
2. Clone the repo:
   ```sh  
   git clone https://github.com/Overboard-RIT/overboard.git
   ```
3. Open the Unity project in **Unity 6**
4. Use `Tools > Build Video AssetBundle` to preload videos.

---

## 🎮 Running the Game

1. **Open** the Unity project
2. Click the **Play** button
3. **Calibrate** and play ([see below](#-calibration-process))

---

## 🎯 Calibration Process

1. **Set up the Kinect sensor** in a fixed position.
2. **Floor Calibration:** Stand in one corner of the play area, then in the opposite corner. Record the player position as it's logged to the debug console.
3. **Use the recorded values** as the `Min Bounds` and `Max Bounds` values in the `Config` script component.

Calibration ensures accurate player tracking but is not required to simply run the game in Unity.

---

## 🚀 Features

### ✅ Plundered and Delivered
- **Survival Gameplay:** Players collect as many doubloons as possible before time runs out.
- **Kinect Motion Tracking:** Players' movement is captured in real-time.
- **Difficulty Scaling:** Modes increase difficulty and add harder mechanics.
- **Leaderboard for High Scores:** Save the highest scores of the day!
- **Metagame Implementation:** Game recognizes when players come back for another round and posts their scores to the metagame API.

### 📋 Uncharted Waters
- **Calibration Menu:** Custom UI to calibrate the space before starting the game.
- **Multiplayer Support:** Players could compete against each other to get the most doubloons.
- **Bigger Game Floor:** Using multiple Kinect sensors, the game floor could be expanded to include more players and more platforms.

---

## 🤝 Contributing

Internal contributions from team members are welcome. See [issues](https://github.com/Overboard-RIT/overboard/issues) and the [roadmap](https://github.com/orgs/Overboard-RIT/projects/1) for work to be done.

### Contribution Guidelines
1. **Branching:** Create a new branch for each feature or bug fix.
2. **Code Reviews:** Submit a pull request for review before merging into the main branch.

---

## 📢 Contact

This project was developed as part of the **New Media Capstone 2025** at **Rochester Institute of Technology (RIT)** and was created for **Imagine RIT 2025**.

### Development Team
- **Andrew Black**
  - [Portfolio](https://blackandrew35.wixsite.com/portfolio) | [LinkedIn](https://www.linkedin.com/in/andrew-black-/) | [GitHub](https://github.com/AB-blackA)
- **Colby Heaton**
  - [Portfolio](https://people.rit.edu/clh5602/portfolio/) | [LinkedIn](https://www.linkedin.com/in/colby-heaton-4a304721b/) | [GitHub](https://github.com/clh5602/)
- **Elia Cohen**
  - [Portfolio](https://maffie.dev/) | [LinkedIn](https://www.linkedin.com/in/maffier/) | [GitHub](https://github.com/maffiemaffie)

### Project Links
- **Instagram:** [@overboard.rit](https://www.instagram.com/overboard.rit)
- **Project Hub:** [Website](https://overboard.framer.website/)

For more information, visit our website and social media.
