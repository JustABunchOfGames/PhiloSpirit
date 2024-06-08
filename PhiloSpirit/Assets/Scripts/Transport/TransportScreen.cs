using Terrain;
using UnityEngine;

public class TransportScreen
{
    public Tile startTile;
    public Tile endTile;

    public TransportScreen(Tile startTile, Tile endTile)
    {
        this.startTile = startTile;
        this.endTile = endTile;
    }
}
