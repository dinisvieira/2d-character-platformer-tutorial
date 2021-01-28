using Godot;
using System;
using System.Collections.Generic;

public class Run : State
{
	[Export]
	public NodePath stateMachinePath;
	
	[Export]
	public float maxSpeedSprint = 900;

	private Move _parentMove;
	
	public Run() : base() {}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
		_parentMove = GetParent() as Move;
	}

	public override void PhysicsProcess(float delta)
	{
		if (Input.IsActionPressed("sprint"))
		{
			_parentMove._maxSpeed.x = maxSpeedSprint;
		}
		else
		{
			_parentMove._maxSpeed.x = _parentMove.maxSpeedDefault.x;
		}

		var player = Owner as Player;
		if(player.IsOnFloor() && _parentMove._velocity.Length() < 1.0)
		{
			_stateMachine.TransitionTo("Move/Idle");
		}
		else if(!player.IsOnFloor())
		{
			_stateMachine.TransitionTo("Move/Air");
		}
		
		_parentMove.PhysicsProcess(delta);
	}

	public override void UnhandledInput(InputEvent @event)
	{
		_parentMove.UnhandledInput(@event);
	}
	
	public override void Enter(IDictionary<string, object> msg = null)
	{
		_parentMove.Enter(msg);
	}
	
	public override void Exit()
	{
		_parentMove.Exit();
	}
}
