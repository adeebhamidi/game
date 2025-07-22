using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlockSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject blockPrefab;
    public Transform[] spawnPositions = new Transform[3];
    
    [Header("Block Types")]
    public BlockType[] availableBlocks = {
        BlockType.Single,
        BlockType.Line2,
        BlockType.Line3,
        BlockType.Line4,
        BlockType.Line5,
        BlockType.LShape,
        BlockType.TShape,
        BlockType.Square2x2,
        BlockType.Square3x3,
        BlockType.Corner
    };
    
    private WoodBlock[] currentBlocks = new WoodBlock[3];
    private GridManager gridManager;
    
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        
        // Ensure we have 3 spawn positions
        if (spawnPositions.Length != 3)
        {
            Debug.LogError("BlockSpawner needs exactly 3 spawn positions!");
        }
    }
    
    public void SpawnNewBlocks()
    {
        // Clear existing blocks
        ClearCurrentBlocks();
        
        // Spawn 3 new random blocks
        for (int i = 0; i < 3; i++)
        {
            SpawnBlockAtPosition(i);
        }
    }
    
    void SpawnBlockAtPosition(int positionIndex)
    {
        if (positionIndex < 0 || positionIndex >= spawnPositions.Length)
            return;
        
        // Choose random block type
        BlockType randomType = availableBlocks[Random.Range(0, availableBlocks.Length)];
        
        // Create block
        GameObject blockObj = Instantiate(blockPrefab, spawnPositions[positionIndex].position, Quaternion.identity);
        WoodBlock block = blockObj.GetComponent<WoodBlock>();
        
        if (block == null)
        {
            block = blockObj.AddComponent<WoodBlock>();
        }
        
        block.blockType = randomType;
        currentBlocks[positionIndex] = block;
    }
    
    public void OnBlockPlaced(WoodBlock placedBlock)
    {
        // Find which position this block was from
        for (int i = 0; i < currentBlocks.Length; i++)
        {
            if (currentBlocks[i] == placedBlock)
            {
                currentBlocks[i] = null;
                break;
            }
        }
        
        // Check if all blocks are placed
        bool allPlaced = true;
        foreach (var block in currentBlocks)
        {
            if (block != null && block.gameObject.activeInHierarchy)
            {
                allPlaced = false;
                break;
            }
        }
        
        // If all blocks are placed, spawn new set
        if (allPlaced)
        {
            Invoke("SpawnNewBlocks", 0.5f); // Small delay for better UX
        }
    }
    
    public bool HasValidMoves()
    {
        if (gridManager == null) return false;
        
        // Check each remaining block to see if it can be placed anywhere on the grid
        foreach (var block in currentBlocks)
        {
            if (block != null && block.gameObject.activeInHierarchy)
            {
                if (CanBlockBePlaced(block))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    bool CanBlockBePlaced(WoodBlock block)
    {
        // Try placing the block at every position on the grid
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (gridManager.CanPlaceBlock(block, row, col))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    void ClearCurrentBlocks()
    {
        for (int i = 0; i < currentBlocks.Length; i++)
        {
            if (currentBlocks[i] != null)
            {
                Destroy(currentBlocks[i].gameObject);
                currentBlocks[i] = null;
            }
        }
    }
    
    // Method to get remaining active blocks (useful for UI or debugging)
    public List<WoodBlock> GetRemainingBlocks()
    {
        List<WoodBlock> remaining = new List<WoodBlock>();
        
        foreach (var block in currentBlocks)
        {
            if (block != null && block.gameObject.activeInHierarchy)
            {
                remaining.Add(block);
            }
        }
        
        return remaining;
    }
    
    // Method to reset all blocks to their original positions (useful for debugging)
    public void ResetAllBlocks()
    {
        foreach (var block in currentBlocks)
        {
            if (block != null)
            {
                block.ResetBlock();
            }
        }
    }
}