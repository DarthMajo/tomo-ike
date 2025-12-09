using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class Map
{
    // Variables
    private Tile[,] map;
    private uint mapSizeX;
    private uint mapSizeY;

    // Properties
    public uint Area
    {
        get { return mapSizeX * mapSizeY; }
    }

    public uint AreaUsable
    {
        get { return (mapSizeX - 1) * (mapSizeY - 1); }
    }

    public uint MapSizeX
    {
        get { return mapSizeX; }
        set { mapSizeX = value; } 
    }

    public uint MapSizeY
    {
        get { return mapSizeY; }
        set { mapSizeY = value; }
    }

    // Constructors
    public Map(uint size_x, uint size_y, double fill)
    {
        mapSizeX = size_x;
        mapSizeY = size_y;
        map = InitMap();
    }

    // Public Functions
    public List<Tile> GetAllTilesOfValue(uint val)
    {
        List<Tile> allValTiles = new List<Tile>();
        for(uint x = 0; x < MapSizeX; x++)
            for(uint y = 0; y < MapSizeY; y++)
                if(GetTile(x, y).Value == val)
                    allValTiles.Add(GetTile(x, y));
        return allValTiles;
    }

    public Tile GetTile(uint x, uint y)
    {
        if(!IsTileInBounds(x, y))
            throw new ArgumentOutOfRangeException(
                $"Tried to access tile {x}x{y} but that is out of bounds."
            );
        return map[y, x];
    }

    public bool IsPerimeterTile(uint x, uint y)
    {
        if(
            x == 0 || x == MapSizeX - 1 || 
            y == 0 || y == MapSizeY - 1
        )
            return true;
        return false;
    }

    public bool IsTileInBounds(uint x, uint y)
    {
        if(x < 0 || x >= MapSizeX)
            return false;
        if(y < 0 || y >= MapSizeY)
            return false;
        return true;
    }

    public void SetTile(uint x, uint y, uint newVal)
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

    // Private Functions
    private Tile[,] InitMap()
    {
        Tile[,] initialMap = new Tile[MapSizeY, MapSizeX];
        for(uint y = 0; y < mapSizeY; y++)
            for(uint x = 0; x < mapSizeX; x++)
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
