using Godot;
using System;

public class CharacterFollow : KinematicBody2D
{
	[Export]
	protected float maxSpeed = 500;

	private Sprite _sprite;
	private Vector2 _velocity = Vector2.Zero;
	private static float DistanceThreshold = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode("TriangleRed") as Sprite;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		var targetGlobalPosition = GetGlobalMousePosition();

		//Stop the agent is it's close to the mouse cursor
		if (GlobalPosition.DistanceTo(targetGlobalPosition) < DistanceThreshold)
		{
			return;
		}

		//move the agent
		_velocity = Steering.Follow(_velocity, GlobalPosition, targetGlobalPosition, maxSpeed: maxSpeed);
		_velocity = MoveAndSlide(_velocity);

		//rotate the agent towards the mouse
		_sprite.Rotation = _velocity.Angle();
	}
}
