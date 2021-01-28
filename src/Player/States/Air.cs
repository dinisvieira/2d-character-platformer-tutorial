using Godot;
using System;
using System.Collections.Generic;

public class Air : State
{
	[Export]
	public NodePath stateMachinePath;
	
	[Export]
	public float accelerationX = 5000;
	
	private Move _parentMove;
	
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
			if (Move.GetMoveDirection().x != 0.0)
			{
				_stateMachine.TransitionTo("Move/Run");
			}
			else if (player.IsOnFloor())
			{
				_stateMachine.TransitionTo("Move/Idle");
			}
		}
	}

	public override void UnhandledInput(InputEvent @event)
	{
		_parentMove.UnhandledInput(@event);
	}
	
	public override void Enter(IDictionary<string, object> msg = null)
	{
		_parentMove.Enter(msg);
		_parentMove._acceleration.x = accelerationX;

		if (msg != null && msg.ContainsKey("velocity") && msg["velocity"] is Vector2 vel)
		{
			_parentMove._velocity = vel;
			_parentMove._maxSpeed.x = Math.Max(vel.x, _parentMove.maxSpeedDefault.x);
		}

		if (msg != null && msg.ContainsKey("impulse") && msg["impulse"] is float imp)
		{
			_parentMove._velocity += CalculateJumpVelocity(imp);
		}
	}

	private Vector2 CalculateJumpVelocity(float impulse)
	{
		var calcVelocity = Move.CalculateVelocity(_parentMove._velocity, _parentMove._maxSpeed, new Vector2(0, impulse), 1, Vector2.Up);
		return calcVelocity;
	}

	public override void Exit()
	{
		_parentMove.Exit();
		_parentMove._acceleration = _parentMove.accelerationDefault;
	}
}
