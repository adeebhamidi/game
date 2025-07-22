# Woodoku - Unity Recreation

A complete recreation of the popular Woodoku puzzle game in Unity. This project combines the strategic gameplay of Sudoku with the block-placement mechanics of Tetris, creating an engaging puzzle experience.

## Game Overview

Woodoku is a puzzle game where players drag and drop wooden blocks onto a 9x9 grid. The objective is to fill complete rows, columns, or 3x3 boxes to clear them and earn points. The game ends when no more blocks can be placed.

### Key Features

- **9x9 Grid System**: Classic Sudoku-style grid with visual feedback
- **10 Different Block Types**: Various wooden block shapes including lines, squares, L-shapes, T-shapes, and corners
- **Line Clearing Mechanics**: Clear complete rows, columns, and 3x3 boxes
- **Scoring System**: Points for placing blocks plus bonuses for clearing lines
- **Game Over Detection**: Automatic detection when no valid moves remain
- **Best Score Tracking**: Persistent high score storage using PlayerPrefs
- **Drag & Drop Interface**: Intuitive mouse-based block placement
- **Visual Preview System**: See where blocks will be placed before dropping

## Project Structure

```
WoodokuUnity/
├── Assets/
│   ├── Scripts/
│   │   ├── GameManager.cs          # Main game controller
│   │   ├── GridManager.cs          # 9x9 grid management
│   │   ├── GridCell.cs             # Individual grid cell behavior
│   │   ├── WoodBlock.cs            # Draggable block system
│   │   └── BlockSpawner.cs         # Block generation and validation
│   ├── Prefabs/
│   │   ├── GridCell.prefab         # Grid cell prefab
│   │   └── WoodBlock.prefab        # Wood block prefab
│   ├── Scenes/
│   │   └── WoodokuGame.unity       # Main game scene
│   └── Materials/                  # (For future material assets)
├── ProjectSettings/
│   └── ProjectSettings.asset       # Unity project configuration
└── README.md                       # This file
```

## Block Types

The game includes 10 different block types:

1. **Single** - 1x1 single block
2. **Line2** - 2-block horizontal line
3. **Line3** - 3-block horizontal line  
4. **Line4** - 4-block horizontal line
5. **Line5** - 5-block horizontal line
6. **LShape** - L-shaped block (3 blocks in L formation)
7. **TShape** - T-shaped block (4 blocks in T formation)
8. **Square2x2** - 2x2 square (4 blocks)
9. **Square3x3** - 3x3 square (9 blocks)
10. **Corner** - Corner piece (3 blocks in corner formation)

## Setup Instructions

### Prerequisites
- Unity 2019.4 LTS or newer
- Basic understanding of Unity interface

### Installation
1. Clone or download this repository
2. Open Unity Hub
3. Click "Add" and select the `WoodokuUnity` folder
4. Open the project in Unity
5. Navigate to `Assets/Scenes/WoodokuGame.unity`
6. Press Play to start the game

### Scene Setup
The main scene includes:
- **Main Camera**: Orthographic camera positioned to view the game area
- **GameManager**: Contains all game logic components
- **GridParent**: Parent object for the 9x9 grid
- **SpawnPositions**: Three positions where new blocks appear

## How to Play

1. **Starting the Game**: Three random wooden blocks appear at the bottom
2. **Placing Blocks**: Click and drag blocks onto the 9x9 grid
3. **Scoring Points**: Earn 10 points per block placed
4. **Clearing Lines**: Complete rows, columns, or 3x3 boxes to clear them
5. **Bonus Points**: Earn 100 points per line and 200 points per box cleared
6. **New Blocks**: After placing all three blocks, three new ones spawn
7. **Game Over**: Game ends when no remaining blocks can be placed

## Code Architecture

### GameManager.cs
- Singleton pattern for global access
- Manages scoring, game states, and UI updates
- Handles line clearing logic and game over detection
- Saves/loads high scores using PlayerPrefs

### GridManager.cs
- Creates and manages the 9x9 grid of cells
- Handles block placement validation
- Manages visual previews and line clearing
- Converts world positions to grid coordinates

### WoodBlock.cs
- Defines different block shapes using Vector2Int arrays
- Implements drag and drop functionality
- Handles mouse input and visual feedback
- Creates dynamic visual representation of blocks

### BlockSpawner.cs
- Spawns three random blocks at designated positions
- Validates if any remaining moves are possible
- Manages block lifecycle and cleanup

### GridCell.cs
- Represents individual cells in the 9x9 grid
- Handles visual state changes (empty, filled, preview)
- Creates sprite renderers dynamically
- Manages colliders for mouse interaction

## Customization

### Adjusting Difficulty
- Modify `availableBlocks` array in BlockSpawner to change block variety
- Adjust scoring values in GameManager inspector
- Change grid size by modifying grid creation loops (currently 9x9)

### Visual Customization
- Modify colors in GridManager inspector
- Change block colors in WoodBlock prefab
- Adjust cell size and spacing in GridManager

### Adding New Block Types
1. Add new enum value to `BlockType`
2. Define shape in `WoodBlock.InitializeShape()`
3. Add to `availableBlocks` array in BlockSpawner

## Technical Notes

- Uses Unity's built-in 2D rendering system
- Sprites are generated procedurally using Texture2D
- Mouse input handled through OnMouse events
- Grid positioning uses world coordinates with orthographic camera
- Game state persisted using Unity's PlayerPrefs system

## Future Enhancements

Potential improvements for the game:
- Sound effects and background music
- Particle effects for line clearing
- Multiple difficulty levels
- Undo functionality
- Touch input for mobile devices
- Animated block placement and clearing
- Different grid sizes (6x6, 12x12)
- Power-ups and special blocks
- Daily challenges and achievements

## Troubleshooting

**Blocks not dragging**: Ensure blocks have Collider2D components and the camera can see them.

**Grid not appearing**: Check that GridManager has a valid cellPrefab reference and gridParent is assigned.

**Game over not detecting**: Verify BlockSpawner.HasValidMoves() is working correctly by checking console for debug messages.

**Score not saving**: Ensure PlayerPrefs has write permissions on your platform.

## License

This project is created for educational purposes. Feel free to use and modify for learning Unity game development.

## Credits

Inspired by the original Woodoku game by Triple Dot Ltd. This is an independent recreation built for learning purposes.