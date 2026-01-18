using TomoIke;

namespace tomo_ike_tests;

public class TestMapGenerator()
{
    [Fact]
    public void TestMapGeneratorSize()
    {
        MapGenerator mg = new MapGenerator(100, 80, 0.85);
        Assert.Equal(100, mg.MapSizeX);
        Assert.Equal(80, mg.MapSizeY);
    }

    [Fact]
    public void TestGenerateMapSeed101()
    {
        MapGenerator mg = new MapGenerator(100, 100, 0.85);
        mg.Generate(101);
    }

    [Fact]
    public void TestGenerateMapSeed2026()
    {
        MapGenerator mg = new MapGenerator(100, 100, 0.85);
        mg.Generate(2026);
    }

    [Fact]
    public void TestGenerateMapSeed420()
    {
        MapGenerator mg = new MapGenerator(100, 100, 0.85);
        mg.Generate(420);
    }


    [Fact]
    public void TestGenerateMapSeeds()
    {
        for(int i = 0; i < 100; i++)
        {
            MapGenerator mg = new MapGenerator(100, 100, 0.85);
            mg.Generate(i);
        }
    }
}