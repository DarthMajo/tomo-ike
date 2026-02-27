using Godot;
using System;
using System.Numerics;

public partial class Player : Sprite2D
{
    private float speed = 64;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("walk_north"))
        {
            Position = new Godot.Vector2(Position.X, Position.Y - (speed * (float)delta));
        }
        if(Input.IsActionPressed("walk_south"))
        {
            Position = new Godot.Vector2(Position.X, Position.Y + (speed * (float)delta));
        }
        if(Input.IsActionPressed("walk_east"))
        {
            Position = new Godot.Vector2(Position.X + (speed * (float)delta), Position.Y);
        }
        if(Input.IsActionPressed("walk_west"))
        {
            Position = new Godot.Vector2(Position.X - (speed * (float)delta), Position.Y);
        }
    }
}
