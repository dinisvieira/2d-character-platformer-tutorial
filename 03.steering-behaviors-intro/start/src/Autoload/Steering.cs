using Godot;
using System;

public class Steering : Node
{

    public static float DefaultMass = 2;
    public static float DefaultMaxSpeed = 400;
    public static float DefaultSlowRadius = 200;

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

    public static Vector2 ArriveTo(Vector2 velocity, Vector2 globalPosition, Vector2 targetPosition, float? maxSpeed = null, float? slowRadius = null, float? mass = null)
    {
        if (maxSpeed == null) { maxSpeed = DefaultMaxSpeed; }
        if (slowRadius == null) { mass = DefaultSlowRadius; }
        if (mass == null) { mass = DefaultMass; }

        var distanceToTarget = globalPosition.DistanceTo(targetPosition);
        var desiredVelocity = (targetPosition - globalPosition).Normalized() * maxSpeed;

        //slowdown when approaching target
        if (distanceToTarget < slowRadius)
        {
            desiredVelocity *= (float)((distanceToTarget / slowRadius) * 0.8 + 0.2); //0.8 + 0.2 makes it a bit faster when it's really close to the target (otherwise it would be really slow)
        }

        var steering = (desiredVelocity - velocity) / mass;
        var newVelocity = velocity + steering;
        return newVelocity ?? Vector2.Zero;
    }
}
