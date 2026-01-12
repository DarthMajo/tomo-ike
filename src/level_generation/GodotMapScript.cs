using Godot;
using System;

public partial class GodotMapScript : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        MapGenerator mg = new MapGenerator(40, 22, 0.75);
		mg.Generate(42);
		Map m = mg.GeneratedMap;

		TileMapLayer tm = (TileMapLayer)GetNode("/root/Level/Map/TileMapLayer");
		buildTilemap(m, tm);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Private Functions
	private void buildTilemap(Map m, TileMapLayer tml)
    {
		for(int x = 0; x < m.MapSizeX; x++)
        {
            for(int y = 0; y < m.MapSizeY; y++)
            {
                int tileVal = (int)m.GetTile(x, y).Value;
				tml.SetCell(new Vector2I(x,y), tileVal, atlasCoords: new Vector2I(0,0));
            }
        }
    }

}
