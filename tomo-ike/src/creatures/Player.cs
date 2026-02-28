using Godot;
using System;
using System.Numerics;
using TomoIke;

public partial class Player : CharacterBody2D
{
        private float speed = 64;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector2 movementDirection = new Godot.Vector2(0, 0);

        if(Input.IsActionPressed("walk_north"))
            movementDirection += new Godot.Vector2(0, -speed);
        if(Input.IsActionPressed("walk_south"))
            movementDirection += new Godot.Vector2(0, speed);
        if(Input.IsActionPressed("walk_east"))
            movementDirection += new Godot.Vector2(speed, 0);
        if(Input.IsActionPressed("walk_west"))
            movementDirection += new Godot.Vector2(-speed, 0);

        Velocity = movementDirection;
        MoveAndSlide();
    }
}
