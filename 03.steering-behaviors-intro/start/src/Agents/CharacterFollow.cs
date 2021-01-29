using Godot;
using System;

public class CharacterFollow : KinematicBody2D
{
	[Export]
	protected float maxSpeed = 500;

	[Export]
	protected NodePath targetPath;

	[Export]
	protected float followOffset = 100;

	private Sprite _sprite;
	private KinematicBody2D _target;
	private Vector2 _velocity = Vector2.Zero;
	private static float ArriveThreshold = 150;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode("TriangleRed") as Sprite;
		_target = GetNode(targetPath) as KinematicBody2D;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (targetPath == null || _target == null)
		{
			SetPhysicsProcess(false);
			return;
		}

		var targetGlobalPosition = _target.GlobalPosition;
		var distanceToTarget = GlobalPosition.DistanceTo(targetGlobalPosition);

		//Stop the agent is it's close to the mouse cursor
		if (distanceToTarget < ArriveThreshold)
		{
			return;
		}

		//If distance to target is bigger than follow offset then set position to target, otherwise use it's own position to basically make it stop
		var followGlobalPosition = GlobalPosition;
		if (distanceToTarget > followOffset)
		{
			followGlobalPosition = targetGlobalPosition - (targetGlobalPosition - GlobalPosition).Normalized() * followOffset;
		}

		//move the agent
		_velocity = Steering.ArriveTo(_velocity, GlobalPosition, followGlobalPosition, maxSpeed: maxSpeed, slowRadius: 200, mass: 20);
		_velocity = MoveAndSlide(_velocity);

		//rotate the agent towards the mouse
		_sprite.Rotation = _velocity.Angle();
	}
}
