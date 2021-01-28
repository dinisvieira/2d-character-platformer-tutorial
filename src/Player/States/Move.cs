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
	public float jumpImpulse = 900;

	public Vector2 _acceleration;
	public Vector2 _maxSpeed;
	public Vector2 _velocity = Vector2.Zero;

	public Move() : base() {}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
		_acceleration = accelerationDefault;
		_maxSpeed = maxSpeedDefault;
	}

	public override void PhysicsProcess(float delta)
	{
		var moveDirection = GetMoveDirection();
		_velocity = CalculateVelocity(_velocity, _maxSpeed, _acceleration, delta, moveDirection);
		var player = Owner as Player;
		_velocity = player.MoveAndSlide(_velocity, Player.FloorNormal);
		
		var events = GetNode("/root/Events"); //TODO: If this doesn'r work I might need to implement the Events class in C#
		events.EmitSignal("player_moved", player);
	}

	public override void UnhandledInput(InputEvent @event)
	{
		
		/*
		if (@event is InputEventMouseMotion eventMouse)
		{
			
		}
		*/
	}
	
	public static Vector2 CalculateVelocity(Vector2 oldVelocity, Vector2 maxSpeed, Vector2 acceleration, float delta, Vector2 moveDirection)
	{
		var newVelocity = oldVelocity;
		
		newVelocity += moveDirection * acceleration * delta;
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
