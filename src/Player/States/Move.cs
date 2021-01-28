using Godot;
using System;
using System.Collections.Generic;

public class Move : State
{
	[Export]
	public NodePath stateMachinePath;
	
	[Export]
	public Vector2 maxSpeedDefault = new Vector2(500, 1500);
	
	[Export]
	public Vector2 accelerationDefault = new Vector2(100000, 3000);
	
	[Export]
	public Vector2 deccelerationDefault = new Vector2(500, 3000);

	[Export]		
	public float jumpImpulse = 900;

	public Vector2 _acceleration;
	public Vector2 _decceleration;
	public Vector2 _maxSpeed;
	public Vector2 _velocity = Vector2.Zero;

	public Move() : base() {}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
		_acceleration = accelerationDefault;
		_decceleration = deccelerationDefault;
		_maxSpeed = maxSpeedDefault;
	}

	public override void PhysicsProcess(float delta)
	{
		var moveDirection = GetMoveDirection();
		_velocity = CalculateVelocity(_velocity, _maxSpeed, _acceleration, _decceleration, delta, moveDirection);
		var player = Owner as Player;
		_velocity = player.MoveAndSlide(_velocity, Player.FloorNormal);
		
		var events = GetNode("/root/Events");
		events.EmitSignal("player_moved", player);
	}

	public override void UnhandledInput(InputEvent @event)
	{
		var player = Owner as Player;
		if (player.IsOnFloor() && @event.IsActionPressed("jump"))
		{
			_stateMachine.TransitionTo("Move/Air", new Dictionary<string, object>(){{"impulse", jumpImpulse}});
		}
	}
	
	public static Vector2 CalculateVelocity(Vector2 oldVelocity, Vector2 maxSpeed, Vector2 acceleration, Vector2 decceleration, float delta, Vector2 moveDirection)
	{
		var newVelocity = oldVelocity;

		newVelocity.y += moveDirection.y * acceleration.y * delta;

		if (moveDirection.x != 0)
		{
			newVelocity.x += moveDirection.x * acceleration.x * delta;
		}
		else if(Math.Abs(newVelocity.x) > 0.1)
		{
			newVelocity.x -= decceleration.x * delta * Math.Sign(newVelocity.x);

			//Check if vector doesn't go in the opposite direction after deccelerating completely. Setting to zero avoids that.
			if (Math.Sign(newVelocity.x) == Math.Sign(oldVelocity.x))
			{
				newVelocity.x = newVelocity.x;
			}
			else
			{
				newVelocity.x = 0;
			}
		}

		newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed.x, maxSpeed.x);
		newVelocity.y= Mathf.Clamp(newVelocity.y, -maxSpeed.y, maxSpeed.y);
		return newVelocity;
	}
	
	public static Vector2 GetMoveDirection()
	{
		var moveRight = Input.GetActionStrength("move_right");
		var moveLeft = Input.GetActionStrength("move_left");
		var movX = moveRight - moveLeft;
		var movY = 1;
		return new Vector2(movX, movY);
	}
}
