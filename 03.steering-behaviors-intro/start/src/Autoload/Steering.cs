using Godot;
using System;

public class Steering : Node
{

    public static float DefaultMass = 2;
    public static float DefaultMaxSpeed = 400;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public static Vector2 Follow(Vector2 velocity, Vector2 globalPosition, Vector2 targetPosition, float? maxSpeed = null, float? mass = null)
    {
        if (maxSpeed == null) { maxSpeed = DefaultMaxSpeed; }
        if (mass == null) { mass = DefaultMass; }

        var desiredVelocity = (targetPosition - globalPosition).Normalized() * maxSpeed;
        var steering = (desiredVelocity - velocity) / mass;
        var newVelocity = velocity + steering;
        return newVelocity ?? Vector2.Zero;
    }
}
