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
    public GameObject grass;
    public GameObject bush;
    public GameObject allBlocks;
    public GameObject allGrass;
    public GameObject allBushes;
    public float blockOffset = 2f;
    public Cell[,] grid;
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
                bool hasGrass;
                if (!isWater)
                {
                    hasGrass = (Random.Range(0, 2) == 1) ? true : false;
                }
                else
                {
                    hasGrass = false;
                }
                Cell cell = new Cell(isWater, hasGrass);
                grid[x, y] = cell;
            }
        }
        spawnBlocks(grid);
        spawnGrass(grid);
        spawnPlayer(grid);
    }

    void spawnBlocks(Cell[,] grid)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    //spawn block
                    var b = Instantiate(block, new Vector3(x + blockOffset, 0, y + blockOffset), Quaternion.identity);
                    b.transform.parent = allBlocks.transform;
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
        } while (grid[randX, randY].isWater);

        _movementScript.Player.transform.position = new Vector3(randX + blockOffset, 3, randY + blockOffset);
        _movementScript.cameraTargetPos = new Vector3(randX + blockOffset + 65, 65, randY + blockOffset - 65);
    }
    public bool CheckNextStep(float dir)
    {
        int x = Mathf.RoundToInt(_movementScript.Player.transform.position.x - blockOffset);
        int y = Mathf.RoundToInt(_movementScript.Player.transform.position.z - blockOffset);

        if (dir == 0)
        {
            if (grid[x, y + 1].isWater)
            {

                return false;
            }
            else return true;
        }
        else
       if (dir == 90)
        {
            if (grid[x + 1, y].isWater)
            {
                return false;
            }
            else return true;

        }
        else
       if (dir == 180)
        {
            if (grid[x, y - 1].isWater)
            {
                return false;
            }
            else return true;

        }
        else
       if (dir == 270)
        {
            if (grid[x - 1, y].isWater)
            {
                return false;
            }
            else return true;
        }
        else
            return false;
    }
    void spawnGrass(Cell[,] grid)
    {
        
        int bushCount = Random.Range(100, 200);
        int x, y;
        for ( x = 0; x < size; x++)
        {
            for ( y = 0; y < size; y++)
            {
                if (grid[x, y].hasGrass)
                {
                    var b = Instantiate(grass, new Vector3(x + blockOffset, -0.5f, y + blockOffset), Quaternion.identity);
                    b.transform.parent = allGrass.transform;
                    
                }
            }
        }
        
        do
        {
            x = Random.Range(0, size);
            y = Random.Range(0, size);
            if (grid[x, y].isWater==false && grid[x,y].hasGrass==false)
            {
                var b = Instantiate(bush, new Vector3(x + blockOffset, 0.5f, y + blockOffset), Quaternion.identity);
                b.transform.parent = allBushes.transform;
                bushCount--;
            }
        } while (bushCount>0);
    }

}