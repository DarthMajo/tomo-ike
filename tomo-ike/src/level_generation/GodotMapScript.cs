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

		// Spawn goal
		PackedScene endScene = GD.Load<PackedScene>("res://map/endDoor.tscn");
		Node2D endDoor = endScene.Instantiate<Node2D>();
		endDoor.Position = SetAssetPixelLocation(m.GoalPositionX, m.GoalPositionY);
		AddChild(endDoor);
		
		// Spawn player
		PackedScene playerScene = GD.Load<PackedScene>("res://objects/creatures/player.tscn");
		Node2D player = playerScene.Instantiate<Node2D>();
		player.Position = SetPlayerLocation(m);
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

	private Godot.Vector2 SetPlayerLocation(Map m)
	{
		float x = m.PlayerSpawnX * GlobalConstants.TILESIZE;
		float y = m.PlayerSpawnY * GlobalConstants.TILESIZE;
		return new Godot.Vector2(x,y);	
	}

	private Godot.Vector2 SetAssetPixelLocation(int tilex, int tiley)
	{
		float x = tilex * GlobalConstants.TILESIZE;
		float y = tiley * GlobalConstants.TILESIZE;
		return new Godot.Vector2(x,y);	
	}
}
