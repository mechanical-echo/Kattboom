using System.Diagnostics;

public class Cell
{
    public bool isWater;
    public bool hasGrass;
    public Cell(bool isWater, bool hasGrass)
    {
        this.isWater = isWater;
        this.hasGrass = hasGrass;
    }
}