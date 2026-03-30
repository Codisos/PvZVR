
public class GridTile
{
    public GridCord cords;
    public Plant placedPlant;

    public GridTile(GridCord cords, Plant plant) { this.cords = cords; placedPlant = plant; }

    public override bool Equals(object obj)
    {
        GridTile tile = obj as GridTile;
        return this.cords.Equals(tile.cords);
    }


}
