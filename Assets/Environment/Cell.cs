
public class Cell
{
    public bool isWater;
    public bool isCoast=false;
    public bool isPassed = false;
    public Cell(bool isWater=true)
    {
        this.isWater = isWater;
    }
    public Cell(Cell cell)
    {
        isWater = cell.isWater;
        isCoast = cell.isCoast;
    }
}
