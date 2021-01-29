using Godot;
using System;

public class CharacterArriveTo : KinematicBody2D
{
	[Export]
	protected float maxSpeed = 500;

	[Export]
	private float slowRadius = 300;

	private Sprite _sprite;
	private Vector2 _velocity = Vector2.Zero;
	private static float DistanceThreshold = 3;

	private Vector2 _targetGlobalPosition;

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("click"))
		{
			_targetGlobalPosition = GetGlobalMousePosition();
			SetPhysicsProcess(true);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode("TriangleRed") as Sprite;
		SetPhysicsProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionPressed("click")) //Update in case there's another click
		{
			_targetGlobalPosition = GetGlobalMousePosition();
		}
		
		//Stop the agent is it's close to the mouse cursor
		if (GlobalPosition.DistanceTo(_targetGlobalPosition) < DistanceThreshold)
		{
			SetPhysicsProcess(false);
			return;
		}

		//move the agent
		_velocity = Steering.ArriveTo(_velocity, GlobalPosition, _targetGlobalPosition, maxSpeed: maxSpeed, slowRadius: slowRadius);
		_velocity = MoveAndSlide(_velocity);

		//rotate the agent towards the mouse
		_sprite.Rotation = _velocity.Angle();
	}
}
