using Godot;
using TomoIke;

public partial class GodotMapScript : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		// First, we will generate the map
        MapGenerator mg = new MapGenerator(40, 22, 0.75);
		mg.Generate(42);
		Map m = mg.GeneratedMap;

		// Set the map tiles to the tilemap
		TileMapLayer tm = (TileMapLayer)GetNode("/root/Level/Map/TileMapLayer");
		BuildTilemap(m, tm);

		// Set map position to have the player in the center of the map
		tm.Position = SetTileMapLocation(m);
		
		// Spawn player
		PackedScene playerScene = GD.Load<PackedScene>("res://objects/creatures/player.tscn");
		Node2D player = playerScene.Instantiate<Node2D>();
		player.Position = SetPlayerLocation();
		AddChild(player);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Private Functions
	private void BuildTilemap(Map m, TileMapLayer tml)
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

	private Godot.Vector2 SetTileMapLocation(Map m)
	{
		float windowXMidpoint = GetViewport().GetVisibleRect().Size.X / 2;
		float windowYMidpoint = GetViewport().GetVisibleRect().Size.Y / 2;
		float x = windowXMidpoint - (m.PlayerSpawnX + 1) * GlobalConstants.TILESIZE + (GlobalConstants.TILESIZE / 2);
		float y = windowYMidpoint - (m.PlayerSpawnY + 1) * GlobalConstants.TILESIZE + (GlobalConstants.TILESIZE / 2);
		return new Godot.Vector2(x, y);
	}

	private Godot.Vector2 SetPlayerLocation()
	{
		return new Godot.Vector2(
			GetViewport().GetVisibleRect().Size.X / 2 - (GlobalConstants.TILESIZE / 2),
			GetViewport().GetVisibleRect().Size.Y / 2 - (GlobalConstants.TILESIZE / 2)
		);	
	}

}
