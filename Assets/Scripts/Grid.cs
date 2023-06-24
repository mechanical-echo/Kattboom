using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Material terrainMaterial;
    public Material edgeMaterial;
    public float waterLevel = .4f;
    public float scale = .1f;
    public int size = 100;
    public GameObject block;
    public float blockOffset= 2f;
    Cell[,] grid;
    public PlayerMovement _movementScript;
    void Start()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }
        spawnBlocks(grid);
       spawnPlayer(grid);
    }

    void spawnBlocks(Cell[,] grid)
    {
        for(int y =0; y<size; y++)
        {
            for(int x =0; x<size; x++)
            {
                Cell cell = grid[x, y];
                if(!cell.isWater)
                {
                    //spawn block
                    var b = Instantiate(block, new Vector3(x + blockOffset, 0, y + blockOffset), Quaternion.identity);
                    b.transform.parent = this.transform;
                }
            }
        }
    }
    void spawnPlayer(Cell[,] grid)
    {
        int randX = 0, randY = 0;
        do
        {
            randX = Random.Range(0, size);
            randY = Random.Range(0, size);
        } while (grid[randX,randY].isWater);

        _movementScript.Player.transform.position = new Vector3(randX+blockOffset, 3, randY+blockOffset);
        _movementScript.cameraTargetPos = new Vector3(randX + blockOffset + 65, 65, randY + blockOffset - 65);
    }
    
}