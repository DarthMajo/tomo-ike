using System;
using System.Collections.Generic;

public class Map
{
	// Variables
	private Tile[,] map;
	private int mapSizeX;
	private int mapSizeY;

	// Properties
	public int Area
	{
		get { return mapSizeX * mapSizeY; }
	}

	public int AreaUsable
	{
		get { return (mapSizeX - 1) * (mapSizeY - 1); }
	}

	public int MapSizeX
	{
		get { return mapSizeX; }
		set { mapSizeX = value; } 
	}

	public int MapSizeY
	{
		get { return mapSizeY; }
		set { mapSizeY = value; }
	}

	// Constructors
	public Map(int size_x, int size_y)
	{
		mapSizeX = size_x;
		mapSizeY = size_y;
		map = InitMap();
	}

	// Public Functions
	public List<Tile> GetAllTilesOfValue(TomoIke.TileType val)
	{
		List<Tile> allValTiles = new List<Tile>();
		for(int x = 0; x < MapSizeX; x++)
			for(int y = 0; y < MapSizeY; y++)
				if(GetTile(x, y).Value == val)
					allValTiles.Add(GetTile(x, y));
		return allValTiles;
	}

	public Tile GetTile(int x, int y)
	{
		if(!IsTileInBounds(x, y))
			return new Tile(TomoIke.TileType.INVALID);
		return map[y, x];
	}

	public bool IsPerimeterTile(int x, int y)
	{
		if(
			x == 0 || x == MapSizeX - 1 || 
			y == 0 || y == MapSizeY - 1
		)
			return true;
		return false;
	}

	public bool IsTileInBounds(int x, int y)
	{
		if(x < 0 || x >= MapSizeX)
			return false;
		if(y < 0 || y >= MapSizeY)
			return false;
		return true;
	}

	public void SetTile(int x, int y, TomoIke.TileType newVal)
	{
		Tile targetTile = GetTile(x, y);
		targetTile.Value = newVal;
	}

	public void SetTile(Tile updatedTile)
	{
		Tile targetTile = GetTile(updatedTile.LocationX,
									updatedTile.LocationY);
		targetTile.Value = updatedTile.Value;
	}

	public override string ToString()
	{
		string str = "";
		for(int y = 0; y < mapSizeY; y++)
		{
			string l = "";
			for(int x = 0; x < mapSizeX; x++)
				l += ((int)map[y,x].Value).ToString("D2") + " ";
			l += "\n";
			str += l;
		}
		return str;
	}

	// Private Functions
	private Tile[,] InitMap()
	{
		Tile[,] initialMap = new Tile[MapSizeY, MapSizeX];
		for(int y = 0; y < mapSizeY; y++)
			for(int x = 0; x < mapSizeX; x++)
			{
				Tile newTile = new Tile
				{
					LocationX = x,
					LocationY = y,
					Value = 0
				};
				initialMap[y, x] = newTile;
			}
		return initialMap;
	}
}
