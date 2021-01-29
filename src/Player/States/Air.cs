using Godot;
using System;
using System.Collections.Generic;

public class Air : State
{
	[Export]
	public NodePath stateMachinePath;
	
	[Export]
	public float accelerationX = 5000;
	
	[Export]		
	public float jumpImpulse = 900;

	private Move _parentMove;

	private int _maxJumpCount = 2;
	private int _currJumpCount = 0;

	public Air() : base() {}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
		_parentMove = GetParent() as Move;
	}

	public override void PhysicsProcess(float delta)
	{
		var player = Owner as Player;
		_parentMove.PhysicsProcess(delta);

		if (player.IsOnFloor())
		{
			if (Move.GetMoveDirection().x == 0.0)
			{
				_stateMachine.TransitionTo("Move/Idle");
			}
			else
			{
				_stateMachine.TransitionTo("Move/Run");
			}
		}
	}

	public override void UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("jump") && _currJumpCount < _maxJumpCount)
		{
			Jump();
		}
		_parentMove.UnhandledInput(@event);
	}

	private void Jump()
	{
		_parentMove._velocity += CalculateJumpVelocity(jumpImpulse);
		_currJumpCount++;
	}

	public override void Enter(IDictionary<string, object> msg = null)
	{
		_parentMove.Enter(msg);
		_parentMove._acceleration.x = accelerationX;

		if (msg != null && msg.ContainsKey("velocity") && msg["velocity"] is Vector2 vel)
		{
			_parentMove._velocity = vel;
			_parentMove._maxSpeed.x = Math.Max(Math.Abs(vel.x), _parentMove._maxSpeed.x);
		}

		if (msg != null && msg.ContainsKey("impulse"))
		{
			Jump();
		}
		else
		{
			_currJumpCount++; //to avoid double jump is player is falling (of a cliff for example)
		}
	}

	private Vector2 CalculateJumpVelocity(float impulse)
	{
		var calcVelocity = Move.CalculateVelocity(_parentMove._velocity, _parentMove._maxSpeed, new Vector2(0, impulse), _parentMove._decceleration, 1, Vector2.Up, _parentMove.maxFallSpeed);
		return calcVelocity;
	}

	public override void Exit()
	{
		_parentMove.Exit();
		_parentMove._acceleration = _parentMove.accelerationDefault;
		_currJumpCount = 0;
	}
}
