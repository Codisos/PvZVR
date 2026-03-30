
public class GridCord
{
    public string column;
    public int row;

    public GridCord(string c, int r) { column = c; row = r; }

    public override bool Equals(object obj)
    {
        GridCord cord = obj as GridCord;
        return this.column == cord.column && this.row == cord.row;
    }
}
